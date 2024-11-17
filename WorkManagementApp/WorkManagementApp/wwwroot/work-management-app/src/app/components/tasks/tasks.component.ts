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
  tasks: any[] = [];
  selectedTask: any = null;

  // Task form fields for creating a new task
  title = '';
  description = '';
  dueDate = '';
  status = '';
  projectId = '';
  assignedUserId = '';
  priority = '';

  constructor(private api: ApiService, private taskService: TaskService) { }

  // Fetch tasks
  getTasks() {
    this.api.getTasks().subscribe((response) => {
      this.tasks = response;
    });
  }

  // Create a new task
  createTask() {
    const newTask = {
      title: this.title,
      description: this.description,
      dueDate: this.dueDate,
      projectId: this.projectId,
      assignedUserId: this.assignedUserId,
      priority: this.priority,
    };
    this.api.createTask(newTask).subscribe(
      (response) => {
        console.log('Task created successfully:', response);
        this.getTasks(); // Refresh the task list
      },
      (error) => {
        console.error('Error creating task:', error);
      }
    );
  }

  // Edit a task
  editTask(task: any) {
    this.selectedTask = { ...task }; // Clone the task object for editing
  }

  // Update a task
  updateTask() {
    this.api.updateTask(this.selectedTask.id, this.selectedTask).subscribe(
      (response) => {
        console.log('Task updated successfully:', response);
        this.selectedTask = null; // Clear the selected task
        this.getTasks(); // Refresh the task list
      },
      (error) => {
        console.error('Error updating task:', error);
      }
    );
  }

  // Cancel editing
  cancelEdit() {
    this.selectedTask = null; // Clear the selected task
  }

  // Delete a task
  deleteTask(taskId: number) {
    if (confirm('Are you sure you want to delete this task?')) {
      this.api.deleteTask(taskId).subscribe(
        () => {
          console.log('Task deleted successfully');
          this.getTasks(); // Refresh the task list
        },
        (error) => {
          console.error('Error deleting task:', error);
        }
      );
    }
  }
}
