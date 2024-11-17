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
  @Input() users: any[] = []; // Input to receive the list of users from the parent component
  @Output() userSelected = new EventEmitter<any>(); // Output to emit the selected user

  selectUser(user: any) {
    this.userSelected.emit(user); // Emit the selected user to the parent component
  }
}
