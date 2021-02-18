import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivityContainerComponent } from './main/activityEntities/activity-container/activity-container.component';
import { ActivityEntitiesComponent } from './main/activityEntities/activityEntities.component';
import { EditActivityComponent } from './main/activityEntities/edit-activity/edit-activity.component';
import { NewActivityComponent } from './main/activityEntities/new-activity/new-activity.component';
import { ViewActivityComponent } from './main/activityEntities/view-activity/view-activity.component';
import { AdminOnlyGuard } from './auth/guards/admin-only.guard';
import { AuthGuard } from './auth/guards/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';
import { ManageUsersComponent } from './main/manage-users/manage-users.component';
import { UserContainerComponent } from './main/manage-users/user-container/user-container.component';
import { EditUserComponent } from './main/manage-users/edit-user/edit-user.component';
import { ViewUserComponent } from './main/manage-users/view-user/view-user.component';
import { RegisterAdminComponent } from './main/manage-users/register-admin/register-admin.component';
import { RegisterComponent } from './auth/register/register.component';
import { ScheduleActivityComponent } from './main/schedule-activity/schedule-activity.component';
import { MyActivitiesComponent } from './main/my-activities/my-activities.component';
import { UserAccountComponent } from './main/user-account/user-account.component';


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
    component: ActivityEntitiesComponent
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
      { path: '', redirectTo: '/not-found', pathMatch: 'full' },
      { path: ':id', component: ViewActivityComponent },
      {
        path: 'edit/:id',
        canActivate: [AuthGuard, AdminOnlyGuard],
        component: EditActivityComponent
      }
    ]
  },
  {
    path: 'manage-users',
    canActivate: [AuthGuard, AdminOnlyGuard],
    component: ManageUsersComponent
  },
  {
    path: 'schedule-activity',
    canActivate: [AuthGuard],
    component: ScheduleActivityComponent
  },
  {
    path: 'manage-user',
    canActivate: [AuthGuard],
    component: UserContainerComponent,
    children: [
      { path: '', redirectTo: '/not-found', pathMatch: 'full' },
      {
        path: ':id',
        canActivate: [AuthGuard, AdminOnlyGuard],
        component: ViewUserComponent
      },
      {
        path: 'edit/:id',
        canActivate: [AuthGuard],
        component: EditUserComponent
      }
    ]
  },
  {
    path: 'my-activities',
    canActivate: [AuthGuard],
    component: MyActivitiesComponent
  },
  {
    path: 'account',
    canActivate: [AuthGuard],
    component: UserAccountComponent
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
