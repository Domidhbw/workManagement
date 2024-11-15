import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
@Injectable({
  providedIn: 'root'
})
export class TaskService {
  constructor(private api: ApiService) { }
  private tasks: any[] = []; // Private property to hold tasks

  fetchTasks() {
    this.api.getTasks().subscribe(
      (response) => {
        console.log('Getting Tasks successful:', response);
        this.tasks = response; // Save tasks in the tasks property
      },
      (error) => {
        console.error('Getting Tasks failed:', error);
      }
    );
  }

  // Method to set tasks
  setTasks(tasks: any[]) {
    this.tasks = tasks;
  }

  // Method to get all tasks
  getTasks(): any[] {
    return this.tasks;
  }

  // Method to add a task
  addTask(task: any) {
    this.tasks.push(task);
  }

  // Method to get a task by ID
  getTaskById(id: number): any | undefined {
    return this.tasks.find((task) => task.id === id);
  }

  // Method to clear all tasks
  clearTasks() {
    this.tasks.forEach((task: { id: number }) => {
      this.api.deleteTask(task.id);
    })
    this.tasks = [];
  }
}
