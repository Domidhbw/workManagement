import { Component } from '@angular/core';
import { ApiService, ProjectService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent {

  title = '';
  description = '';
  dueDate = '';
  status = '';
  projectId = '';
  assignedUserId = '';
  priority = '';

  tasks: any[] = []; // Property to hold tasks

  constructor(private api: ApiService, private taskService: TaskService) { }

  getTasks() {
    this.taskService.fetchTasks();
    this.tasks = this.taskService.getTasks();
  }
  createTask() {
    this.api.createTask({ title: this.title, description: this.description, dueDate: this.dueDate, projectId: this.projectId, assignedUserId: this.assignedUserId, priority: this.priority }).subscribe(
      (response) => {
        console.log('Creation of Task successful:', response);
      },
      (error) => {
        console.error('Creation of Task failed:', error);
      }
    );
  }
}
