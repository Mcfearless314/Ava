import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) {
  }

  createProject(organisationId: string, title: string, subTitle: string, projectManagerId: string) {
    return this.http.post<{ projectId: string; message: string }>(
      `/api/Project/create`,
      { title, subTitle, organisationId, projectManagerId }
    );
  }
}
