import { CommonModule } from "@angular/common";
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { TokenService } from "../../services/token.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [CommonModule, ReactiveFormsModule],
  standalone: true
})
export class RegisterComponent {
  registerForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(50),
      // Add a validator that has at least one special character: Password must contain at least one special character. Valid special characters include: ! @ # $ % ^ & * ( ) _ + - = { } [ ] | \\\\ : ; , . ? /
      Validators.pattern(/^(?=.*[!@#$%^&*()_+\-={}\[\]|\\:;,.?\/]).*$/)
    ])
  });

  registerSuccess = false;
  registerError = false;

  constructor(private authService: AuthService, private tokenService: TokenService) { }

  onSubmit() {
    if (this.registerForm.valid) {
      const username = this.registerForm.value.username || '';
      const password = this.registerForm.value.password || '';

      this.authService.register(username, password).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.tokenService.setToken(response.token);
          // Store token or redirect user
          this.registerSuccess = true;
          this.registerError = false;
        },
        error: (error) => {
          console.error('Registration failed', error);
          // Handle registration error
          this.registerError = true;
          this.registerSuccess = false;
        }
      });
    }
  }
}
