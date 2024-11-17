import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api: ApiService) { }

  private users: any[] = [];

  setUsers(users: any[]) {
    this.users = users;
    users.forEach(user => {
      this.users.push(user);
    });
  }






}
