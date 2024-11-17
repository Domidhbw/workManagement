import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { ApiService, ProjectService, TaskService } from '../../services';
@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})

export class ProjectsComponent {
  constructor(private projectService: ProjectService, private api: ApiService, private router: Router) { }

  name = '';
  description = '';
  startDate = '';
  endDate = '';
  managerId = '';
  assignedUserId = '';

  display: any[] = []; 

  getProjects() {
    this.projectService.fetchProjectsByUser();
    this.display = this.projectService.getProjects();
  }

  displayTasksForProject(projectId: number) {
    if (projectId != null) {
      const taskService = this.projectService.getTaskServiceForProject(projectId);
      console.log(`Tasks for project ${projectId}:`, taskService.getTasks());
      return taskService.getTasks();
    }
    return;
  }
  createProject() {
    this.api.createProject({ name: this.name, description: this.description, startDate: this.startDate, endDate: this.endDate, managerId : this.managerId, assignedUserId: this.assignedUserId }).subscribe(
      (response) => {
        console.log('Creation of Project successful:', response);
      },
      (error) => {
        console.error('Creation of Project failed:', error);
      }
    );
  }

  navigateToTasks(projectId: number) {
    this.router.navigate(['/tasks'], { queryParams: { projectId } });
  }

  isDeadlinePassed(endDate: string): boolean {
    const currentDate = new Date();
    const projectEndDate = new Date(endDate);
    return projectEndDate < currentDate;
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('token');
  }

  logout() {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userId');
    this.router.navigate(['/login']);
  }
}
