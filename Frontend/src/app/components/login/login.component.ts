import { CommonModule } from "@angular/common";
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { ToastService } from '../../services/toast.service';
import { Router } from '@angular/router';

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

  constructor(private authService: AuthService, private tokenService: TokenService, private router: Router, private toastService: ToastService) { }

  async onSubmit() {
    if (this.loginForm.valid) {
      const username = this.loginForm.value.username || '';
      const password = this.loginForm.value.password || '';

      this.authService.login(username, password).subscribe({
        next: async (response) => {
          this.tokenService.setToken(response.token);
          this.loginSuccess = true;
          this.loginError = false;

          const userId = this.tokenService.getUserIdFromToken();
          if (!userId) {
            this.loginError = true;
            return;
          }

          this.authService.getOrganisationId(userId).subscribe({
            next: async (organisation) => {
              try {
                if(!organisation || !organisation.organisationId) {
                  await this.router.navigate(['/organisation/createNewOrganisation']);
                } else {
                  await this.router.navigate([`/organisation/${organisation.organisationId}`]);
                }
              } catch (err) {
                this.loginError = true;
              }
            },
            error: (err) => {
              this.loginError = true;
            }
          });
        },
        error: (error) => {
          this.loginError = true;
          this.loginSuccess = false;
          if(error.status === 429) {
            this.toastService.show(error.error.error);
          }
        }
      });
    }
  }
}
