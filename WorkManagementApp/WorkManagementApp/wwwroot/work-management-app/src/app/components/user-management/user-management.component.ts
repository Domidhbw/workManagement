import { Component, OnInit } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';
import { UserListComponent} from '../user-list/user-list.component'


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
 
  constructor(private api: ApiService) { }

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
    })
  }
  onUserSelected(user: any) {
    this.selectedUser = user; // Handle the selected user
    console.log('Selected User:', user);
  }
}
