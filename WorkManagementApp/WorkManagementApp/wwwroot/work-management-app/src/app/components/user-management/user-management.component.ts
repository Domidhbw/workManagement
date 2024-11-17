import { Component, OnInit } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, RouterModule } from '@angular/router';
import { UserListComponent } from '../user-list/user-list.component';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule, UserListComponent],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  users: any[] = [];
  selectedUser: any = null;
  newUser: any = { name: '', email: '', password: '', role: '' };
  searchTerm: string = '';

  constructor(private api: ApiService, private router: Router) { } 

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    this.api.getUsers().subscribe({
      next: (response) => {
        this.users = response;
        console.log('users:', response);
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    });
  }

  onUserSelected(user: any) {
    this.selectedUser = user; 
    console.log('Selected User:', user);
  }

  updateUser(userId: number, updatedData: any): void {
    this.api.updateUser(userId, updatedData).subscribe({
      next: (response) => {
        console.log('User updated:', response);
        this.fetchUsers(); 
      },
      error: (error) => {
        console.error('Error updating user:', error);
      }
    });
  }

  deleteUser(userId: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.api.deleteUser(userId).subscribe({
        next: () => {
          console.log('User deleted:', userId);
          this.fetchUsers(); 
        },
        error: (error) => {
          console.error('Error deleting user:', error);
        }
      });
    }
  }

  filteredUsers() {
    if (!this.searchTerm) {
      return this.users;
    }
    const lowerCaseSearchTerm = this.searchTerm.toLowerCase();
    return this.users.filter(user =>
      user.username.toLowerCase().includes(lowerCaseSearchTerm) ||
      user.email.toLowerCase().includes(lowerCaseSearchTerm)
    );
  }

  switchToProjectPage() {
    this.router.navigate(['/projects']); 
  }
}
