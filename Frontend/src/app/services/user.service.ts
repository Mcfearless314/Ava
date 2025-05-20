import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {User} from "../models/user.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {
  }

  addUserToOrganisation(organisationId: string, userName: string): Observable<User> {
    return this.http.post<{ username: string, id: string }>(
      `/api/Organisation/addUser`,
      { organisationId, userName }
    );
  }
}
