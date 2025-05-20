import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganisationService } from 'src/app/services/organisation.service';
import {forkJoin} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {Project} from "../../models/project.model";
import {User} from "../../models/user.model";

@Component({
  selector: 'app-organisation-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './organisation-page.component.html',
  styleUrls: ['./organisation-page.component.css']

})



export class OrganisationPageComponent implements OnInit {
  projects: Project[] = [];
  users: User[] = [];

  constructor(private organisationService: OrganisationService, private route: ActivatedRoute, private router: Router) {}


  async goToProject(projectId: string) {
     await this.router.navigate([`/project/${projectId}`]);
  }
  ngOnInit() {

    const organisationId = this.route.snapshot.paramMap.get('organisationId');

    if (!organisationId) {
      console.error('No organisation ID found!');
      return;
    }

    forkJoin({
      projects: this.organisationService.getProjects(organisationId.toString()),
      users: this.organisationService.getUsers(organisationId.toString())
    }).subscribe({
      next: (data) => {
        this.projects = data.projects;
        this.users = data.users;
      },
      error: (err) => {
        console.error('Error loading organisation data:', err);
      }
    });
  }
}
