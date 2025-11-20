import { Component, Input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/authService';
import { finalize } from 'rxjs';

@Component({
  selector: 'header-component',
  imports: [RouterLink],
  templateUrl: './header-component.html',
  styleUrl: './header-component.css',
})
export class HeaderComponent {
  @Input() authorized!: boolean | null;
  @Input() backUrl: string | null = null;

  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  logout() {
    this.authService
      .logout()
      .pipe(
        finalize(() => {
          this.router.navigate(['/']);
        })
      )
      .subscribe({
        error: (err) => {
          console.error(err);
        },
      });
  }
}
