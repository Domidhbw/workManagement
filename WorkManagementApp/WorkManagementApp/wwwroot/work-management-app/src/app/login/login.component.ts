import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-login',
  standalone: true, imports: [CommonModule, FormsModule],
  template: `
    <div>
      <h2>Login</h2>
      <input [(ngModel)]="username" placeholder="Username" />
      <input [(ngModel)]="password" type="password" placeholder="Password" />
      <button (click)="login()">Login</button>
    </div>
  `,
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private api: ApiService) { }

  login() {
    this.api.login({ username: this.username, password: this.password }).subscribe(
      (response) => {
        console.log('Login successful:', response);
      },
      (error) => {
        console.error('Login failed:', error);
      }
    );
  }
}
