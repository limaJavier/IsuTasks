import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Task } from '../../models/task';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { TaskService } from '../../services/taskService';
import { AuthService } from '../../../../shared/services/authService';
import { handleServerError } from '../../utils/errorHandling';

@Component({
  selector: 'app-upsert-task-component',
  imports: [ReactiveFormsModule, HeaderComponent],
  templateUrl: './upsert-task-component.html',
  styleUrl: './upsert-task-component.css',
})
export class UpsertTaskComponent implements OnInit {
  task!: Task | null;
  form!: FormGroup;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly taskService: TaskService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      const taskId = params.get('id');

      if (taskId) {
        this.taskService.getTaskById(taskId).subscribe({
          next: (task) => {
            this.task = task;
            this.setFormGroup();
          },
          error: (err) => handleServerError(err, this.authService, this.router),
        });
      } else {
        this.task = null;
        this.setFormGroup();
      }
    });
    this.setFormGroup();
  }

  submitForm() {
    if (this.form.valid) {
      const task = {
        id: this.task ? this.task.id : '',
        title: this.form.value.taskTitle,
        description: this.form.value.taskDescription,
        dueDate: new Date(this.form.value.taskDueDate),
        isCompleted: this.form.value.taskIsCompleted,
      };

      if (this.task) {
        this.taskService.updateTask(task).subscribe({
          error: (err) => handleServerError(err, this.authService, this.router),
        });
      } else {
        this.taskService.createTask(task).subscribe({
          error: (err) => handleServerError(err, this.authService, this.router),
        });
      }

      this.router.navigate(['tasks']);
    }
  }

  private setFormGroup() {
    this.form = new FormGroup({
      taskTitle: new FormControl(this.task?.title, Validators.required),
      taskDescription: new FormControl(this.task?.description, Validators.required),
      taskDueDate: new FormControl(
        this.task ? this.task.dueDate.toISOString().slice(0, 16) : null,
        Validators.required
      ),
      taskIsCompleted: new FormControl(this.task ? this.task.isCompleted : false),
    });
  }
}
