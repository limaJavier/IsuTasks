import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthRequest } from '../models/authRequest';
import { AuthResponse } from '../models/authResponse';
import { Injectable } from '@angular/core';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { catchError, finalize, map, Observable, of, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private static readonly _apiUrl = 'http://localhost:9090'; // TODO: get api url from environment
  private readonly accessTokenKey = 'accessTokenKey';
  private readonly refreshTokenKey = 'refreshTokenKey';

  constructor(private readonly httpClient: HttpClient) {}

  register(request: AuthRequest): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(AuthService._apiUrl + '/auth/register', request).pipe(
      tap((response) => {
        this.saveAuthTokens(response.accessToken, response.refreshToken);
      })
    );
  }

  login(request: AuthRequest): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(AuthService._apiUrl + '/auth/login', request).pipe(
      tap((response) => {
        this.saveAuthTokens(response.accessToken, response.refreshToken);
      })
    );
  }

  logout(): Observable<Object> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) return throwError(() => new Error('Cannot resolve refresh token'));

    return this.httpClient
      .post(
        AuthService._apiUrl + '/auth/logout',
        { refreshToken: refreshToken },
        this.getAuthorizationHeader()
      )
      .pipe(
        finalize(() => {
          this.removeAuthTokens(); // Remove auth tokens
        })
      );
  }

  refreshIfForced(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.removeAuthTokens();
      return throwError(() => new Error('Cannot resolve refresh-token'));
    }

    const accessToken = this.getAccessToken();
    if (!accessToken) {
      this.removeAuthTokens();
      return throwError(() => new Error('Cannot resolve access-token'));
    }

    if (this.isAccessTokenExpired(accessToken)) return this.refresh(refreshToken);

    return of({ accessToken: accessToken, refreshToken: refreshToken });
  }

  isAuth(): boolean {
    return this.getRefreshToken() ? true : false;
  }

  private refresh(refreshToken: string): Observable<AuthResponse> {
    return this.httpClient
      .post<AuthResponse>(
        AuthService._apiUrl + '/auth/refresh',
        { refreshToken: refreshToken },
        this.getAuthorizationHeader()
      )
      .pipe(
        tap((response) => {
          this.saveAuthTokens(response.accessToken, response.refreshToken);
        }),
        catchError((err) => {
          this.removeAuthTokens();
          return throwError(() => err);
        })
      );
  }

  private isAccessTokenExpired(accessToken: string): boolean {
    try {
      const payload: JwtPayload = jwtDecode(accessToken);
      const now = Math.floor(Date.now() / 1000);
      return payload.exp! < now;
    } catch {
      return true;
    }
  }

  private saveAuthTokens(accessToken: string, refreshToken: string) {
    localStorage.setItem(this.accessTokenKey, accessToken);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }

  private removeAuthTokens() {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
  }

  private getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  private getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  private getAuthorizationHeader() {
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${this.getAccessToken()}`,
      }),
    };
  }
}
