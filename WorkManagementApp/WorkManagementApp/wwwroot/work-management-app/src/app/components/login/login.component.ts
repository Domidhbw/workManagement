import { Component } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private api: ApiService) { }

  login() {
    this.api.login({ username: this.username, password: this.password }).subscribe(
      (response) => {
        console.log('Login successful:', response);
        sessionStorage.setItem('token', response.token);
        sessionStorage.setItem('userId', response.userId.toString());
      },
      (error) => {
        console.error('Login failed:', error);
      }
    );
  }
}
