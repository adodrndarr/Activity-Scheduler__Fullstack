import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivityEntitiesComponent } from './activityEntities/activityEntities.component';
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
  { path: 'activities', component: ActivityEntitiesComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '', redirectTo: '/activities', pathMatch: 'full' },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
