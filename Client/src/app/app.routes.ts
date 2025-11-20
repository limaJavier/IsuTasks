import { Routes } from '@angular/router';
import { ListTasksComponent } from './features/tasks/pages/list-tasks-component/list-tasks-component';
import { DetailTaskComponent } from './features/tasks/pages/detail-task-component/detail-task-component';
import { NotFoundComponent } from './features/home/pages/not-found-component/not-found-component';
import { HomeComponent } from './features/home/pages/home-component/home-component';
import { UpsertTaskComponent } from './features/tasks/pages/upsert-task-component/upsert-task-component';
import { AuthComponent } from './features/home/pages/auth-component/auth-component';
import { authGuard } from './shared/services/autGuard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: AuthComponent },
  { path: 'login', component: AuthComponent },
  { path: 'tasks', component: ListTasksComponent, canActivate: [authGuard] },
  { path: 'tasks/:id/details', component: DetailTaskComponent, canActivate: [authGuard] },
  { path: 'tasks/create', component: UpsertTaskComponent, canActivate: [authGuard] },
  { path: 'tasks/:id/update', component: UpsertTaskComponent, canActivate: [authGuard] },
  { path: '**', component: NotFoundComponent },
];
