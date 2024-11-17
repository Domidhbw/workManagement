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

  getUsers(): Observable<any> {
    return this.http.get(`${this.baseUrl}/User`);
  }

  getUser(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/User/${userId}`);
  }

  updateUser(userId: number, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/User/${userId}`, data);
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/User/${userId}`);
  }

  getProjects(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Projects`);
  }

  createProject(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Projects`, data);
  }

  deleteProject(projectId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Projects/${projectId}`);
  }

  updateProject(projectId: number, data:any): Observable<any> {
    return this.http.put(`${this.baseUrl}/Projects/${projectId}`, data);
  }

  getTasks(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Tasks`);
  }

  createTask(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Tasks`, data);
  }
  updateTask(taskId: number, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/Tasks/${taskId}`, data);
  }

  deleteTask(taskId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Tasks/${taskId}`);
  }
  getTasksByProjectId(projectId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Tasks/project/${projectId}`);
  }
}
