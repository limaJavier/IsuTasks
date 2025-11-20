import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { AuthService } from '../../../../shared/services/authService';

@Component({
  selector: 'app-auth-component',
  imports: [ReactiveFormsModule, HeaderComponent],
  templateUrl: './auth-component.html',
  styleUrl: './auth-component.css',
})
export class AuthComponent implements OnInit {
  @Input() register!: boolean;
  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)]),
  });

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.url.subscribe((segments) => {
      const fullPath = (segments || []).map((s) => s.path).join('/');

      if (fullPath.includes('register')) {
        this.register = true;
      } else if (fullPath.includes('login')) {
        this.register = false;
      }
    });
  }

  submitForm() {
    if (this.form.valid) {
      if (this.register) {
        this.authService
          .register({
            email: this.form.value.email!,
            password: this.form.value.password!,
          })
          .subscribe({
            next: (_) => {
              this.router.navigate(['/tasks']);
            },
            error: (err) => console.error(err),
          });
      } else {
        this.authService
          .login({
            email: this.form.value.email!,
            password: this.form.value.password!,
          })
          .subscribe({
            next: (_) => {
              this.router.navigate(['/tasks']);
            },
            error: (err) => console.error(err),
          });
      }
    }
  }
}
