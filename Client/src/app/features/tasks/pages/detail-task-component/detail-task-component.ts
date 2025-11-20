import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../../../shared/components/header-component/header-component';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Task } from '../../models/task';
import { TaskService } from '../../services/taskService';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-detail-task-component',
  imports: [HeaderComponent],
  templateUrl: './detail-task-component.html',
  styleUrl: './detail-task-component.css',
})
export class DetailTaskComponent implements OnInit {
  task!: Task;
  constructor(
    private readonly route: ActivatedRoute,
    private readonly taskService: TaskService,
    private readonly router : Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      const taskId = params.get('id')!;
      this.taskService.getTaskById(taskId).subscribe({
        next: (task) => (this.task = task),
        error: (err) => {
          if(err instanceof HttpErrorResponse)
          {
            if(err.status === HttpStatusCode.NotFound)
            {
              this.router.navigate(["/notfound"])
            }
          }
        }
      });
    });
  }
}
