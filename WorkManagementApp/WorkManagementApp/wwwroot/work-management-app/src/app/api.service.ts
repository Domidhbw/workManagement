import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = 'https://localhost:7267/api';

  constructor(private http: HttpClient) { }

  register(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Auth/register`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Auth/login`, data);
  }

  getProjects(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Projects`);
  }

  createProject(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Projects`, data);
  }

  getTasks(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Tasks`);
  }

  createTask(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Tasks`, data);
  }
}
