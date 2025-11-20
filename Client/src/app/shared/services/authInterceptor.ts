import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from './authService';

@Injectable({
  providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Avoid intercepting the auth calls
    if (request.url.includes('/auth')) {
      return next.handle(request);
    }

    return this.authService.refreshIfForced().pipe(
      switchMap((response) => {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${response.accessToken}`,
          },
        });
        return next.handle(request);
      }),
      catchError((err) => {
        console.error(err);
        return next.handle(request);
      })
    );
  }
}
