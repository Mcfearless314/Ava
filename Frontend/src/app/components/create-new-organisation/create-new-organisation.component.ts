import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpClient, HttpClientModule, HttpErrorResponse} from '@angular/common/http';
import { Router } from '@angular/router';
import {OrganisationService} from "../../services/organisation.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-create-new-organisation',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './create-new-organisation.component.html',
  styleUrls: ['./create-new-organisation.component.css']
})
export class CreateNewOrganisationComponent {
  organisationName = '';
  successMessage = '';
  errorMessage = '';

  constructor(private organisationService: OrganisationService, private http: HttpClient, private router: Router) {}

  createOrganisation(): void {
    this.organisationService.createOrganisation(this.organisationName).subscribe({
      next: async (response) => {
        this.successMessage = response.message;
        this.errorMessage = '';
        setTimeout(async () => {
          await this.router.navigate(['/organisation', response.organisationId]);
        }, 2000);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = 'Error creating organisation. Reason: ' + error.error.error;
        this.successMessage = '';
      }
    });
  }
}
