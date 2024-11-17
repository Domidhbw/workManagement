import { Component, OnInit } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  users: any[] = [];
  selectedUser: any = null;
  newUser: any = { name: '', email: '', password: '', role: '' };
  userId: any = null;
  user: any = null;
  constructor(private api: ApiService) { }

  ngOnInit(): void {
    // Hole die userId aus dem SessionStorage
    this.userId = sessionStorage.getItem('userId');
    console.log('User ID:', this.userId);

    if (this.userId) {
      // Lade den User
      this.api.getUser(this.userId).subscribe({
        next: (response) => {
          this.user = response;
          console.log('User loaded:', this.user);

          // Prüfe die Benutzerberechtigung erst nach erfolgreichem Laden
          if (this.checkUserAuthority()) {
            this.loadUsers();
            console.log('User is admin');
          } else {
            console.error('User is not an admin');
          }
        },
        error: (error) => {
          console.error('Error loading user:', error);
        }
      });
    } else {
      console.error('No userId found in sessionStorage');
    }
  }

  loadUsers(): void {
    this.api.getUsers().subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    })
  }

  checkUserAuthority(): boolean {
    // Sicherstellen, dass `roles` existiert
    if (this.user && this.user.roles && this.user.roles.includes('Admin')) {
      return true;
    }
    return false;
  }

}
