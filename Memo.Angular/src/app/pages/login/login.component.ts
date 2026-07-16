import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: false,
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = '';
  password = '';
  errorMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Заполните все поля';
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    this.authService.login(data).subscribe({
      next: (response) => {
        console.log('Login response:', response); // <-- Проверь, что приходит
        this.authService.saveToken(response.token); // <-- ЭТО ГЛАВНОЕ!
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.errorMessage = 'Неверный email или пароль';
      }
    });
  }
}
