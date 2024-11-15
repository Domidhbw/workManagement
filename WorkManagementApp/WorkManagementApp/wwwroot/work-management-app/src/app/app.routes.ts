import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProjectsComponent } from './projects/projects.component';
import { TasksComponent } from './tasks/tasks.component';

export const appRoutes: Routes = [
  { path: '', component: LoginComponent }
];
