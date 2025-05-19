import { CommonModule } from "@angular/common";
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    password: new FormControl('', [Validators.required, Validators.maxLength(50)])
  });

  loginSuccess = false;
  loginError = false;

  constructor(private authService: AuthService, private tokenService: TokenService) { }

  onSubmit() {
    if (this.loginForm.valid) {
      const username = this.loginForm.value.username || '';
      const password = this.loginForm.value.password || '';

      this.authService.login(username, password).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          this.tokenService.setToken(response.token);
          // Store token or redirect user
          this.loginSuccess = true;
          this.loginError = false;
        },
        error: (error) => {
          console.error('Login failed', error);
          // Handle login error
          this.loginError = true;
          this.loginSuccess = false;
        }
      });
    }
  }
}
