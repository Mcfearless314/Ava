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

    constructor(private userService: UserService, private projectService: ProjectService, private organisationService: OrganisationService, private route: ActivatedRoute, private router: Router) {
    }


    async goToProject(projectId: string) {
        await this.router.navigate([`/project/${projectId}`]);
    }

    ngOnInit() {

        this.organisationId = this.route.snapshot.paramMap.get('organisationId');

        if (!this.organisationId) {
            console.error('No organisation ID found!');
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
                console.error('Error loading organisation data:', err);
            }
        });
    }

    goToCreateProject() {
        this.projectService.createProject(
            this.organisationId!,
            this.newProjectTitle,
            this.newProjectSubTitle,
            this.selectedManager
        ).subscribe({
            next: (data) => {
                console.log('Project created successfully:', data);
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
                console.error('Error creating project:', err);
            }
        });
    }

    addUser() {
        this.userService.addUserToOrganisation(this.organisationId!, this.newUserUsername).subscribe({
            next: (data) => {
                console.log('User added successfully:', data);
                this.users.push({
                    id: data.id,
                    username: data.username,
                });
                this.newUserUsername = '';
                this.errorMessage = null; // Clear any previous errors
            },
            error: (err: HttpErrorResponse) => {
                console.error('Error adding user:', err.error.error);
                this.errorMessage = err.error?.error || 'An unexpected error occurred.';
            }
        });
    }
}
