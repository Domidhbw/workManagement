import { Component, OnInit } from '@angular/core';
import { ApiService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, RouterModule, ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router'; // Import Router
import { UserListComponent } from '../user-list/user-list.component';

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
  userId = '';
  tasks: any[] = [];
  filteredTasks: any[] = [];
  selectedProjectId: number | null = null;
  selectedTask: any = null;
  taskStatuses = Object.values(TaskStatus);
  title = '';
  description = '';
  dueDate = '';
  status = '';
  projectId = '';
  assignedUserId = '';
  priority = '';
  taskComments: { [taskId: number]: { id: number; commentText: string; createdAt: string; userId: number }[] } = {};
  newComment: { [taskId: number]: string } = {}; // For new comment input per task
  showCreateForm = false; // Added variable to control the visibility of the create task form

  constructor(
    private api: ApiService,
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['projectId']) {
        this.selectedProjectId = +params['projectId'];
        this.filterTasksByProject();
      }
    });
    this.getTasks();
    const userString = sessionStorage.getItem('user');

    if (userString) {
      const user = JSON.parse(userString);
      this.userId = user.id;
    }
  }

  getStatusName(status: string | number): string {
    const statusMapping: { [key: string]: string } = {
      '0': 'Not Started',
      '1': 'In Progress',
      '2': 'Completed',
      '3': 'Blocked'
    };
    return statusMapping[status.toString()] || 'Unknown Status';
  }

  getTasks() {
    this.api.getTasks().subscribe((response) => {
      this.tasks = response;
      this.filterTasksByProject();
    });
  }

  filterTasksByProject() {
    if (this.selectedProjectId !== null) {
      this.filteredTasks = this.tasks.filter(task => task.projectId === this.selectedProjectId);
    } else {
      this.filteredTasks = this.tasks;
    }
  }

  fetchTaskComments(taskId: number) {
    this.api.getTaskComment(taskId).subscribe(
      (comments) => {
        this.taskComments[taskId] = comments.length > 0 ? comments : []; // Initialize as empty array if no comments
      },
      (error) => {
        console.error(`Error fetching comments for task ${taskId}:`, error);
        this.taskComments[taskId] = []; // Handle error by initializing as empty array
      }
    );
  }

  addTaskComment(taskId: number) {
    const commentData = { commentText: this.newComment[taskId], userid: this.userId };
    this.api.createTaskComment(taskId, commentData).subscribe(
      (comment) => {
        if (!this.taskComments[taskId]) {
          this.taskComments[taskId] = [];
        }
        this.taskComments[taskId].push(comment); // Append the new comment
        this.newComment[taskId] = ''; // Clear input field
      },
      (error) => {
        console.error(`Error adding comment to task ${taskId}:`, error);
      }
    );
  }

  deleteTaskComment(taskId: number, commentId: number) {
    this.api.deleteTaskComment(taskId, commentId).subscribe(
      () => {
        this.taskComments[taskId] = this.taskComments[taskId].filter(comment => comment.id !== commentId);
      },
      (error) => {
        console.error(`Error deleting comment ${commentId} for task ${taskId}:`, error);
      }
    );
  }

  setSelectedProject(projectId: number) {
    this.selectedProjectId = projectId;
    this.filterTasksByProject();
  }

  getTasksByStatus(status: string) {
    return this.filteredTasks.filter(task => task.status.toString() === status);
  }

  createTask() {
    if (this.selectedProjectId) {
      this.projectId = this.selectedProjectId.toString();
    }

    const newTask = {
      title: this.title,
      description: this.description,
      dueDate: this.dueDate,
      status: Number(this.status),
      projectId: Number(this.projectId),
      assignedUserId: Number(this.assignedUserId),
      priority: Number(this.priority),
    };
    console.log(newTask);

    this.api.createTask(newTask).subscribe(
      (response) => {
        console.log('Task created successfully:', response);
        this.getTasks();
        this.resetForm(); // Reset the form after creation
        this.showCreateForm = false; // Hide the form after creation
      },
      (error) => {
        console.error('Error creating task:', error);
      }
    );
  }

  resetForm() {
    this.title = '';
    this.description = '';
    this.dueDate = '';
    this.status = '0'; // Default status
    this.projectId = '';
    this.assignedUserId = '';
    this.priority = '';
  }

  showCreateTaskForm() {
    this.showCreateForm = true; // Show the form when the button is clicked
  }

  cancelCreate() {
    this.resetForm(); // Reset the form fields
    this.showCreateForm = false; // Hide the form
  }

  editTask(task: any) {
    this.selectedTask = { ...task };
    if (typeof this.selectedTask.status === 'string') {
      this.selectedTask.status = parseInt(this.selectedTask.status, 10);
    }
  }

  updateTask() {
    if (typeof this.selectedTask.status === 'string') {
      this.selectedTask.status = parseInt(this.selectedTask.status, 10);
    }

    this.api.updateTask(this.selectedTask.id, this.selectedTask).subscribe(
      (response) => {
        this.selectedTask = null;
        this.getTasks();
      },
      (error) => {
        console.error('Error updating task:', error);
      }
    );
  }

  cancelEdit() {
    this.selectedTask = null;
  }

  deleteTask(taskId: number) {
    if (confirm('Are you sure you want to delete this task?')) {
      this.api.deleteTask(taskId).subscribe(
        () => {
          this.getTasks();
        },
        (error) => {
          console.error('Error deleting task:', error);
        }
      );
    }
  }

  switchToProjectPage() {
    this.router.navigate(['/projects']);
  }
}
