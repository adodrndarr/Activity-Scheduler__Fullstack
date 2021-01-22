import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivityContainerComponent } from './activityEntities/activity-container/activity-container.component';
import { ActivityEntitiesComponent } from './activityEntities/activityEntities.component';
import { EditActivityComponent } from './activityEntities/edit-activity/edit-activity.component';
import { NewActivityComponent } from './activityEntities/new-activity/new-activity.component';
import { ViewActivityComponent } from './activityEntities/view-activity/view-activity.component';
import { AdminOnlyGuard } from './auth/guards/admin-only.guard';
import { AuthGuard } from './auth/guards/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterAdminComponent } from './auth/register-admin/register-admin.component';
import { RegisterComponent } from './auth/register/register.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';


const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  {
    path: 'register-admin',
    canActivate: [AuthGuard, AdminOnlyGuard],
    component: RegisterAdminComponent
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'activities',
    canActivate: [AuthGuard],
    component: ActivityEntitiesComponent,
  },
  {
    path: 'create-activity',
    canActivate: [AuthGuard, AdminOnlyGuard],
    component: NewActivityComponent
  },
  {
    path: 'activity-entity',
    canActivate: [AuthGuard],
    component: ActivityContainerComponent,
    children: [
      { path: '', redirectTo: '/activities', pathMatch: 'full' },
      { path: ':id', component: ViewActivityComponent },
      {
        path: 'edit/:id',
        canActivate: [AuthGuard, AdminOnlyGuard],
        component: EditActivityComponent
      }
    ]
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: '', redirectTo: '/activities', pathMatch: 'full' },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
