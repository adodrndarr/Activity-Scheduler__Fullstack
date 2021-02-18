import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './auth/auth.component';
import { HeaderComponent } from './header/header.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { ActivityEntitiesComponent } from './main/activityEntities/activityEntities.component';
import { CookieService } from 'ngx-cookie-service';
import { ErrorComponent } from './shared/error/error.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';
import { FooterComponent } from './footer/footer.component';
import { GlobalErrorHandlerService } from './services/global-error-handler.service';
import { NewActivityComponent } from './main/activityEntities/new-activity/new-activity.component';
import { ViewActivityComponent } from './main/activityEntities/view-activity/view-activity.component';
import { EditActivityComponent } from './main/activityEntities/edit-activity/edit-activity.component';
import { ActivityContainerComponent } from './main/activityEntities/activity-container/activity-container.component';
import { ManageUsersComponent } from './main/manage-users/manage-users.component';
import { UserContainerComponent } from './main/manage-users/user-container/user-container.component';
import { EditUserComponent } from './main/manage-users/edit-user/edit-user.component';
import { ViewUserComponent } from './main/manage-users/view-user/view-user.component';
import { RegisterAdminComponent } from './main/manage-users/register-admin/register-admin.component';
import { ScheduleActivityComponent } from './main/schedule-activity/schedule-activity.component';
import { MyActivitiesComponent } from './main/my-activities/my-activities.component';
import { UserAccountComponent } from './main/user-account/user-account.component';


@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    HeaderComponent,
    RegisterComponent,
    LoginComponent,
    ActivityEntitiesComponent,
    ErrorComponent,
    LoadingSpinnerComponent,
    NotFoundComponent,
    FooterComponent,
    NewActivityComponent,
    ViewActivityComponent,
    EditActivityComponent,
    ActivityContainerComponent,
    ManageUsersComponent,
    UserContainerComponent,
    EditUserComponent,
    ViewUserComponent,
    RegisterAdminComponent,
    ScheduleActivityComponent,
    MyActivitiesComponent,
    UserAccountComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandlerService
    },
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
