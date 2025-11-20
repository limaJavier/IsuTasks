import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { Task } from '../../models/task';
import { TaskService } from '../../services/taskService';
import { AuthService } from '../../../../shared/services/authService';
import { handleServerError } from '../../utils/errorHandling';

@Component({
  selector: 'app-list-tasks-component',
  imports: [HeaderComponent, RouterLink],
  templateUrl: './list-tasks-component.html',
  styleUrl: './list-tasks-component.css',
})
export class ListTasksComponent implements OnInit {
  tasks!: Task[];
  constructor(
    private readonly router: Router,
    private readonly taskService: TaskService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  goToUpsertTask(taskId: string | null = null) {
    if (taskId) this.router.navigate(['tasks', taskId, 'update']);
    else this.router.navigate(['tasks', 'create']);
  }

  toggleTask(task: Task): void {
    this.taskService.updateTask({... task, isCompleted: !task.isCompleted}).subscribe({
      next: () => this.loadTasks(), 
      error: (err) => handleServerError(err, this.authService, this.router),
    });
  }

  deleteTask(taskId: string): void {
    this.taskService.deleteTask(taskId).subscribe({
      next: () => this.loadTasks(),
      error: (err) => handleServerError(err, this.authService, this.router),
    });
  }

  private loadTasks(): void {
    this.taskService.getTasks().subscribe({
      next: (tasks) => (this.tasks = tasks),
      error: (err) => handleServerError(err, this.authService, this.router),
    });
  }
}
