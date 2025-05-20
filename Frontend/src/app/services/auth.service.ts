import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginResponse {
  message: string;
  token: string;
}
export interface OrganisationResponse {
  organisationId: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '/api/Auth';

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { username, password });
  }

  register(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/register`, { username, password });
  }

  getOrganisationId(userId: string): Observable<OrganisationResponse> {
    return this.http.get<OrganisationResponse>(`/api/User/getOrganisationId/${userId}`);
  }
}
