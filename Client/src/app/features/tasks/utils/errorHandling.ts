import { Router } from '@angular/router';
import { AuthService } from '../../../shared/services/authService';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

export function handleServerError(err: unknown, authService: AuthService, router: Router) {
  console.error(err);
  if (err instanceof HttpErrorResponse) {
    if (err.status == HttpStatusCode.Unauthorized) {
      authService.logout().subscribe({
        error: (err) => console.error(err),
      });
      router.navigate(['/']);
    }
  }
}
