import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, Router } from '@angular/router'; 
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  username = '';
  password = '';

  constructor(private api: ApiService, private router: Router) { } 
  ngOnInit(): void {

  }
  login() {
    this.api.login({ username: this.username, password: this.password }).subscribe(
      (response) => {
        console.log('Login successful:', response);
        sessionStorage.setItem('token', response.token);
        sessionStorage.setItem('userId', response.userId.toString());
        this.saveCurrentUser();
        this.router.navigate(['/projects']);
      },
      (error) => {
        console.error('Login failed:', error);
      }
    );
  }

  saveCurrentUser() {
    this.api.getUser(Number(sessionStorage.getItem('userId'))).subscribe((response) => {
      console.log('User:', response);
      sessionStorage.setItem('user', JSON.stringify(response));
    }
    )
  }

}
