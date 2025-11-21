import { map, Observable } from 'rxjs';
import { Task } from '../models/task';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private static readonly _apiUrl = 'http://localhost:9090'; // TODO: get api url from environment

  constructor(private readonly httpClient: HttpClient) {}

  getTaskById(id: string): Observable<Task> {
    return this.httpClient.get<Task>(TaskService._apiUrl + `/tasks/${id}`).pipe(
      map((task) => {
        task.dueDate = new Date(task.dueDate);
        return task;
      })
    );
  }

  getTasks(): Observable<Task[]> {
    return this.httpClient.get<Task[]>(TaskService._apiUrl + '/tasks').pipe(
      map((tasks) => {
        tasks.forEach((task) => (task.dueDate = new Date(task.dueDate)));
        return tasks;
      })
    );
  }

  createTask(task: Omit<Task, 'id'>): Observable<Object> {
    return this.httpClient.post(TaskService._apiUrl + '/tasks', task);
  }

  updateTask(task: Task): Observable<Object> {
    return this.httpClient.put(TaskService._apiUrl + '/tasks', task);
  }

  deleteTask(id: string): Observable<Object> {
    return this.httpClient.delete(TaskService._apiUrl + `/tasks/${id}`);
  }
}
