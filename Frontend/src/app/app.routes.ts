import { Routes } from '@angular/router';
import { OrganisationPageComponent } from './components/organisation-page/organisation-page.component';
import {HomePageComponent} from "./components/home-page/home-page.component";
import {ProjectPageComponent} from "./components/project-page/project-page.component";

export const appRoutes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'organisation/:organisationId', component: OrganisationPageComponent },
  { path: 'project/:projectId', component: ProjectPageComponent },
  { path: '**', redirectTo: '' }
];
