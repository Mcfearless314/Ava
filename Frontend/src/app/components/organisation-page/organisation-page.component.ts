import {Component, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {OrganisationService} from 'src/app/services/organisation.service';
import {forkJoin} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {Project} from "../../models/project.model";
import {User} from "../../models/user.model";
import {FormsModule} from "@angular/forms";
import {ProjectService} from "../../services/project.service";
import {UserService} from "../../services/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {ToastService} from "../../services/toast.service";

@Component({
  selector: 'app-organisation-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './organisation-page.component.html',
  styleUrls: ['./organisation-page.component.css']

})


export class OrganisationPageComponent implements OnInit {
  organisationId: string | null = null;
  projects: Project[] = [];
  users: User[] = [];
  newProjectTitle: string = '';
  newProjectSubTitle: string = '';
  selectedManager: string = '';
  managers: User[] = [];
  newUserUsername: string = '';
  errorMessage: string | null = null;

  constructor(private toastService: ToastService, private userService: UserService, private projectService: ProjectService, private organisationService: OrganisationService, private route: ActivatedRoute, private router: Router) {
  }


  async goToProject(projectId: string) {
    this.projectService.checkIfUserCanAccessProject(projectId).subscribe(response => {
      if (response.hasAccess) {
        this.router.navigate([`/project/${projectId}`]);
      } else {
        this.toastService.show('Access denied to this project.');
        return;
      }
    });
  };

  ngOnInit() {

    this.organisationId = this.route.snapshot.paramMap.get('organisationId');

    if (!this.organisationId) {
      this.toastService.show('Organisation ID is missing.');
      return;
    }

    forkJoin({
      projects: this.organisationService.getProjects(this.organisationId.toString()),
      users: this.organisationService.getUsers(this.organisationId.toString())
    }).subscribe({
      next: (data) => {
        this.projects = data.projects;
        this.users = data.users;
        this.managers = data.users;
      },
      error: (err) => {
        this.toastService.show('Error occurred.');
      }
    });
  }

  createProject() {
    this.projectService.createProject(
      this.organisationId!,
      this.newProjectTitle,
      this.newProjectSubTitle,
      this.selectedManager
    ).subscribe({
      next: (data) => {
        this.toastService.show('Project created successfully.');
        this.projects.push({
          projectId: data.projectId,
          title: this.newProjectTitle,
          subTitle: this.newProjectSubTitle,
        });
        this.newProjectTitle = '';
        this.newProjectSubTitle = '';
        this.selectedManager = '';
      },
      error: (err) => {
        if (err.status === 403) {
          this.toastService.show('You do not have permission to create a project in this organisation.');
        } else {
          this.toastService.show('Error occurred:' + err.error.error);
        }
      }
    });
  }

  addUser() {
    this.userService.addUserToOrganisation(this.organisationId!, this.newUserUsername).subscribe({
      next: (data) => {
        this.toastService.show('User added successfully.');
        this.users.push({
          id: data.id,
          username: data.username,
        });
        this.newUserUsername = '';
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === 400) {
          this.toastService.show('User already exists.');
        } else if (err.status === 404) {
          this.toastService.show('User not found.');
        } else if (err.status === 403) {
          this.toastService.show('You do not have permission to add a user to this organisation.');
        } else {
          this.toastService.show('Error occurred:' + err.error.error);
        }
      }
    });
  }
}
