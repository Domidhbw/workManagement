import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  @Input() users: any[] = []; 
  @Output() userSelected = new EventEmitter<any>(); 

  selectUser(user: any) {
    this.userSelected.emit(user); 
  }

  searchTerm: string = '';

  filteredUsers() {
    if (!this.searchTerm) {
      return this.users;
    }
    const lowerCaseSearchTerm = this.searchTerm.toLowerCase();
    return this.users.filter(user =>
      user.username.toLowerCase().includes(lowerCaseSearchTerm)
    );
  }

}
