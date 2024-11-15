import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
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
  constructor(private projectService: ProjectService, private api: ApiService) { }

  name = '';
  description = '';
  startDate = '';
  endDate = '';
  managerId = '';
  assignedUserId = '';

  display: any[] = []; 
  // Fetch all projects and initialize their TaskServices
  getProjects() {
    this.projectService.fetchProjects();
    this.display = this.projectService.getProjects();
  }

  // Display tasks for a specific project
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

}
