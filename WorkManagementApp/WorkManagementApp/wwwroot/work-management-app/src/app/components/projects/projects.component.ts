import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterOutlet, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { ApiService, ProjectService, TaskService } from '../../services';
import { UserListComponent } from '../user-list/user-list.component'
@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule, UserListComponent],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})

export class ProjectsComponent implements OnInit {
  selectedProject: any = null;
  name = '';
  description = '';
  startDate = '';
  endDate = '';
  managerId = '';
  assignedUser = '';
  assignedUserId: number | null = null; // Selected user's ID
  assignedManager = '';
  assignedManagerId: number | null = null; // Selected user's ID
  assignedUserEdit = '';
  assignedManagerEdit = '';
  projects: any[] = [];
  isAddingProject: boolean = false; // Track whether the "Add Project" form is visible
  users: any[] = []; // List of users to display in the user list
  isProjectManager: boolean = false;
  isAdmin: boolean = false;
  constructor(private projectService: ProjectService, private api: ApiService, private router: Router) { }

  ngOnInit(): void {
    this.getProjects();
    this.getUsers();
    this.canAddProject();
    this.canSwitchToUserPage();
  }

  getProjects() {
    this.projectService.fetchProjectsByUser();
    this.projects = this.projectService.getProjects();
  }

  showAddProjectForm() {
    this.isAddingProject = true;  // Show the Add Project form
  }
  getUsers() {
    this.api.getUsers().subscribe(
      (response) => {
        this.users = response;
      }
    );
  }
  // Handle user selection from UserListComponent
  onUserSelectedAdd(user: any) {
    this.assignedUserId = user.id; // Set the selected user's ID
    this.assignedUser = user.username;
  }
  onManagerSelectedAdd(manager: any) {
    this.assignedManagerId = manager.id; 
    this.assignedManager = manager.username;
  }
  onUserSelectedEdit(user: any) {
    this.selectedProject.assignedUserId = user.id; // Set the selected user's ID
    this.assignedUserEdit = user.username;
  }
  onManagerSelectedEdit(manager: any) {
    this.selectedProject.managerId = manager.id;
    this.assignedManagerEdit = manager.username;
  }
  createProject() {
    const newProject = {
      name: this.name,
      description: this.description,
      startDate: this.startDate,
      endDate: this.endDate,
      managerId: this.assignedManagerId,
      assignedUserId: this.assignedUserId
    };

    this.api.createProject(newProject).subscribe(
      (response) => {
        console.log('Creation of Project successful:', response);
        this.isAddingProject = false; // Hide the form after successful creation
        this.getProjects(); // Refresh the project list
      },
      (error) => {
        console.error('Creation of Project failed:', error);
      }
    );
  }

  cancelAddProject() {
    this.isAddingProject = false; // Hide the form
  }

  editProject(project: any) {
    if (this.selectedProject && this.selectedProject.id === project.id) {
      this.cancelEdit();
    } else {
      this.selectedProject = { ...project }; 
    }
  }


  deleteProject(projectId: number) {
    if (confirm('Are you sure you want to delete this Project?')) {
      this.api.deleteProject(projectId).subscribe(
        () => {
          console.log('Project deleted successfully');
          this.getProjects();
        },
        (error) => {
          console.error('Error deleting project:', error);
        }
      );
    }
  }

  updateProject() {
    this.api.updateProject(this.selectedProject.id, this.selectedProject).subscribe(
      (response) => {
        console.log('Project updated successfully:', response);
        this.selectedProject = null;
        this.getProjects();
      },
      (error) => {
        console.error('Error updating project:', error);
      }
    );
  }

  cancelEdit() {
    this.selectedProject = null;
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

  canAddProject() {
    const userString = sessionStorage.getItem('user');

    if (userString) {
      const user = JSON.parse(userString);
      console.log(user.roles);
      if (user.roles && user.roles.includes('Projectmanager')) {
        this.isProjectManager = true;
      }
    }
  }

  getProgressPercentage(project: any): number {
    if (project.startDate && project.endDate) {
      const startDate = new Date(project.startDate).getTime();
      const endDate = new Date(project.endDate).getTime();
      const currentDate = new Date().getTime();

      if (currentDate >= endDate) {
        return 100;
      }
      const totalDuration = endDate - startDate;
      const elapsed = currentDate - startDate;

      const percentage = (elapsed / totalDuration) * 100;
      return Math.min(100, Math.max(0, Math.round(percentage))); 
    }
    return 0; 
  }


  getProgressColor(project: any): string {
    const percentage = this.getProgressPercentage(project);
    if (percentage < 50) {
      return 'red';
    } else if (percentage < 75) {
      return 'yellow';
    } else {
      return 'green';
    }
  }

  onUserSelected(event: any) {
    console.log('User selected:', event);
    this.assignedUserId = event; 
  }

  navigateToTasks(projectId: number) {
    this.router.navigate(['/tasks'], { queryParams: { projectId } });
  }

  canSwitchToUserPage() {
    const userString = sessionStorage.getItem('user');

    if (userString) {
      const user = JSON.parse(userString);
      console.log(user.roles);
      if (user.roles && user.roles.includes('Admin')) {
        this.isAdmin = true;
      }
    }
  }

  switchToUserPage() {
    this.router.navigate(['/userManagement']);
  }
}
