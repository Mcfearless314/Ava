import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {TokenService} from "./token.service";
import {Observable} from "rxjs";
import {User} from "../models/user.model";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient, private tokenService: TokenService) {
  }

  createProject(organisationId: string, title: string, subTitle: string, projectManagerId: string) {
    return this.http.post<{ projectId: string; message: string }>(
      `/api/Project/create`,
      {title, subTitle, organisationId, projectManagerId}
    );
  }

  checkIfUserCanAccessProject(projectId: string) {
    const userId = this.tokenService.getUserIdFromToken();
    return this.http.get<{ hasAccess: boolean }>(
      `/api/Project/checkAccess/${projectId}/${userId}`
    )
  }

  addUserToProject(projectId: string, id: string): Observable<User> {
    return this.http.post<User>(`/api/Project/addUserToProject/${projectId}/${id}`, null);
  }

}
