import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Project} from "../models/project.model";
import {User} from "../models/user.model";


@Injectable({
  providedIn: 'root'
})
export class OrganisationService {
  constructor(private http: HttpClient) {
  }

  getProjects(organisationId: string): Observable<Project[]> {
    return this.http.get<Project[]>(`/api/Organisation/getAllProjects/${organisationId}`);
  }

  getUsers(organisationId: string): Observable<User[]> {
    return this.http.get<User[]>(`/api/User/getUsers/${organisationId}`);
  }

  createOrganisation(name: string): Observable<{ organisationId: string; message: string }> {
    return this.http.post<{ organisationId: string; message: string }>(
      '/api/Organisation/create',
      {name}
    );
  }

  getOrganisationId(projectId: string) : Observable<string> {
    return this.http.get<string>(`/api/Organisation/getOrganisationByProject/${projectId}`);
  }
}
