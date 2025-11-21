import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Task } from '../../models/task';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { TaskService } from '../../services/taskService';
import { AuthService } from '../../../../shared/services/authService';
import { handleServerError } from '../../utils/errorHandling';
import { getValidationMessage } from '../../../../shared/utils/formValidation';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-upsert-task-component',
  imports: [ReactiveFormsModule, HeaderComponent],
  templateUrl: './upsert-task-component.html',
  styleUrl: './upsert-task-component.css',
})
export class UpsertTaskComponent implements OnInit {
  task!: Task | null;
  form!: FormGroup;
  validationMessage: string | null = null;

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
    this.validationMessage = null;

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
          next: () => this.router.navigate(['tasks']),
          error: (err) => this.handleError(err),
        });
      } else {
        this.taskService.createTask(task).subscribe({
          next: () => this.router.navigate(['tasks']),
          error: (err) => this.handleError(err),
        });
      }
    } else {
      this.validationMessage = this.getValidationMessage();
    }
  }

  private setFormGroup() {
    this.form = new FormGroup({
      taskTitle: new FormControl(this.task?.title, [Validators.required, Validators.maxLength(200)]),
      taskDescription: new FormControl(this.task?.description, [Validators.required, Validators.maxLength(500)]),
      taskDueDate: new FormControl(
        this.task ? this.task.dueDate.toISOString().slice(0, 16) : null,
        Validators.required
      ),
      taskIsCompleted: new FormControl(this.task ? this.task.isCompleted : false),
    });
  }

  private getValidationMessage() {
    return getValidationMessage(
      [
        {
          field: 'taskTitle',
          type: 'required',
          message: 'Title is a required field',
        },
        {
          field: 'taskTitle',
          type: 'maxlength',
          message: 'Title can be at most 200 characters',
        },
        {
          field: 'taskDescription',
          type: 'required',
          message: 'Description is a required field',
        },
        {
          field: 'taskDescription',
          type: 'maxlength',
          message: 'Description can be at most 500 characters',
        },
        {
          field: 'taskDueDate',
          type: 'required',
          message: 'Due Date is a required field',
        },
      ],
      this.form
    );
  }

  private handleError(err: Error) {
    if (
      err instanceof HttpErrorResponse &&
      err.status !== HttpStatusCode.InternalServerError &&
      err.status !== HttpStatusCode.Unauthorized
    ) {
      this.validationMessage = err.error.title;
    }

    handleServerError(err, this.authService, this.router);
  }
}
