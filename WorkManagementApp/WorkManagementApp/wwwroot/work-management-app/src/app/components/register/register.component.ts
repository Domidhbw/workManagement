import { Component } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  role = '2';

  constructor(private api: ApiService) { }

  register() {
    this.api.register({ username: this.username,email: this.email, password: this.password, role: this.role }).subscribe(
      (response) => {
        console.log('registration successful:', response);
      },
      (error) => {
        console.error('Login failed:', error);
      }
    );
  }
}
