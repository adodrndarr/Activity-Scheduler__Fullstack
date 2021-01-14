import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './auth/auth.component';
import { HeaderComponent } from './header/header.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegisterAdminComponent } from './auth/register-admin/register-admin.component';
import { AuthInterceptorService } from './auth/auth-interceptor.service';
import { ActivityEntitiesComponent } from './activityEntities/activityEntities.component';
import { CookieService } from 'ngx-cookie-service';
import { ErrorComponent } from './shared/error/error.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { NotFoundComponent } from './shared/not-found/not-found.component';
import { FooterComponent } from './footer/footer.component';
import { GlobalErrorHandlerService } from './services/global-error-handler.service';


@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    HeaderComponent,
    RegisterComponent,
    LoginComponent,
    RegisterAdminComponent,
    ActivityEntitiesComponent,
    ErrorComponent,
    LoadingSpinnerComponent,
    NotFoundComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
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
