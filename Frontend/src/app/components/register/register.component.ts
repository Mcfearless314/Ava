import { CommonModule } from "@angular/common";
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from "@angular/forms";
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
      Validators.pattern(/^(?=.*[!@#$%^&*()_+\-={}\[\]|\\:;,.?\/]).*$/)
    ]),
    confirmPassword: new FormControl('', Validators.required)
  }, { validators: this.passwordsMatchValidator });

  registerSuccess = false;
  registerError = false;

  passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { passwordsMismatch: true };
  }

  hasPasswordMismatchError(): boolean {
    return (
      this.registerForm.hasError('passwordsMismatch') &&
      !!this.registerForm.get('confirmPassword')?.touched
    );
  }
  constructor(private authService: AuthService, private tokenService: TokenService) { }

  onSubmit() {
    if (this.registerForm.valid) {
      const username = this.registerForm.value.username || '';
      const password = this.registerForm.value.password || '';


      this.authService.register(username, password).subscribe({
        next: (response) => {
          this.tokenService.setToken(response.token);
          this.registerSuccess = true;
          this.registerError = false;
        },
        error: (error) => {
          this.registerError = true;
          this.registerSuccess = false;
        }
      });
    }
    else {
      this.registerError = true;
      this.registerSuccess = false;
    }
  }
}
