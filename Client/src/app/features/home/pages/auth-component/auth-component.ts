import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { AuthService } from '../../../../shared/services/authService';
import { getValidationMessage } from '../../../../shared/utils/formValidation';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

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
  validationMessage: string | null = null;

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
    this.validationMessage = null;
    
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
            error: (err) => this.handleError(err),
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
            error: (err) => this.handleError(err),
          });
      }
    } else {
      this.validationMessage = this.getValidationMessage();
    }
  }

  private getValidationMessage() {
    return getValidationMessage(
      [
        {
          field: 'password',
          type: 'required',
          message: 'Password is a required field',
        },
        {
          field: 'password',
          type: 'minlength',
          message: 'Password must be 8 characters at least',
        },
        {
          field: 'email',
          type: 'required',
          message: 'Email is a required field',
        },
        {
          field: 'email',
          type: 'email',
          message: 'Email must be in a valid format',
        },
      ],
      this.form
    );
  }

  private handleError(err: Error) {
    if (err instanceof HttpErrorResponse && err.status !== HttpStatusCode.InternalServerError) {
      this.validationMessage = err.error.title;
    }
  }
}
