import { Component, OnInit } from '@angular/core';
import { ApiService, TaskService } from '../../services';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, RouterModule, ActivatedRoute } from '@angular/router';

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

  constructor(private api: ApiService, private taskService: TaskService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['projectId']) {
        this.selectedProjectId = +params['projectId'];
        this.filterTasksByProject();
      }
    });
    this.getTasks();
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

  setSelectedProject(projectId: number) {
    this.selectedProjectId = projectId;
    this.filterTasksByProject();
  }

  getTasksByStatus(status: string) {
    return this.filteredTasks.filter(task => task.status.toString() === status);
  }

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
        this.getTasks();
      },
      (error) => {
        console.error('Error creating task:', error);
      }
    );
  }

  editTask(task: any) {
    this.selectedTask = { ...task };
  }

  updateTask() {
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

}


