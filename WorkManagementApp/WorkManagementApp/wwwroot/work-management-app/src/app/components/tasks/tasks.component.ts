import { Component, OnInit } from '@angular/core';
import { ApiService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, RouterModule } from '@angular/router';

enum TaskStatus {
  NotStarted = '0',
  InProgress = '1',
  Completed = '2',
  Blocked = '3'
}

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  tasks: any[] = [];
  filteredTasks: any[] = [];
  selectedProjectId: number | null = null; // Variable zum Filtern nach Projekten
  selectedTask: any = null;
  taskStatuses = Object.values(TaskStatus);

  // Task form fields for creating a new task
  title = '';
  description = '';
  dueDate = '';
  status = '';
  projectId = '';
  assignedUserId = '';
  priority = '';

  constructor(private api: ApiService, private taskService: TaskService) { }

  ngOnInit(): void {
    this.getTasks();
  }

  // Fetch tasks
  getTasks() {
    this.api.getTasks().subscribe((response) => {
      console.log(response);
      this.tasks = response;
      this.filterTasksByProject(); // Filter tasks by the selected project when loaded
    });
  }

  // Filter tasks by the selected project
  filterTasksByProject() {
    if (this.selectedProjectId !== null) {
      this.filteredTasks = this.tasks.filter(task => task.projectId === this.selectedProjectId);
    } else {
      this.filteredTasks = this.tasks;
    }
  }

  // Set the selected project ID and filter tasks
  setSelectedProject(projectId: number) {
    this.selectedProjectId = projectId;
    this.filterTasksByProject();
  }

  // Get tasks by status for the filtered tasks
  getTasksByStatus(status: string) {
    return this.filteredTasks.filter(task => task.status.toString() === status);
  }

  // Create a new task
  createTask() {
    const newTask = {
      title: this.title,
      description: this.description,
      dueDate: this.dueDate,
      status: this.status,
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
