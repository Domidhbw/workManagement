import { Injectable } from '@angular/core';
import { TaskService } from './task.service';
import { ApiService } from './api.service';
@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private api: ApiService) { }

  private projects: any[] = []; // Array to store all projects
  private projectTasks: { [projectId: number]: TaskService } = {}; // Map project ID to its TaskService


  fetchProjects() {
    this.api.getProjects().subscribe(
      (projects) => {
        console.log('Projects fetched successfully:', projects);
        this.setProjects(projects);

        // Fetch tasks for each project
        projects.forEach((project: { id: number; }) => {
          this.api.getTasksByProjectId(project.id).subscribe(tasks => {
            const taskService = this.getTaskServiceForProject(project.id);
            taskService.setTasks(tasks);
          });
        });
      },
      (error) => {
        console.error('Failed to fetch projects:', error);
      }
    );
  }

  setProjects(projects: any[]) {
    this.projects = projects;
    projects.forEach(project => {
      // Initialize a TaskService for each project
      if (!this.projectTasks[project.id]) {
        this.projectTasks[project.id] = new TaskService(this.api);
      }
    });
  }

  // Method to get all projects
  getProjects(): any[] {
    return this.projects;
  }

  // Method to get a project by ID
  getProjectById(projectId: number): any | undefined {
    return this.projects.find(project => project.id === projectId);
  }

  // Get the TaskService for a specific project
  getTaskServiceForProject(projectId: number): TaskService {
    if (!this.projectTasks[projectId]) {
      this.projectTasks[projectId] = new TaskService(this.api);
    }
    return this.projectTasks[projectId];
  }

  // Clear all projects and tasks
  clearProjects() {

    this.projects.forEach((project: { id: number; }) => {
      this.api.deleteProject(project.id);
    }) 


    this.projects = [];
    this.projectTasks = {};
  }
}
