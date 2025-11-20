import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { AuthService } from '../../../../shared/services/authService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-component',
  imports: [HeaderComponent],
  templateUrl: './home-component.html',
  styleUrl: './home-component.css',
})
export class HomeComponent implements OnInit {
  constructor(private readonly authService: AuthService, private readonly router: Router) {}

  ngOnInit(): void {
    if (this.authService.isAuth()) this.router.navigate(['/tasks']);
  }
}
