import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/Entities/Models/user.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';


@Component({
  selector: 'app-view-user',
  templateUrl: './view-user.component.html'
})
export class ViewUserComponent implements OnInit {
  constructor(
    private dataStorageService: DataStorageService,
    private helperService: HelperService,
    private route: ActivatedRoute,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private authService: AuthService
  ) { }


  user: User;
  id: string;
  errorMessage: string;
  isLoading = false;
  isAdmin = false;

  ngOnInit() {
    this.initializeId();
    this.getUsers();
    this.checkUserRole();
  }

  private getUsers(): void {
    this.isLoading = true;
    if (this.dataStorageService.users.length > 0) {
      this.user = this.getUserById();
      this.isLoading = false;
      return;
    }

    this.httpService.getUsers()
      .subscribe(newUsers => {

        this.dataStorageService.users = newUsers;
        this.user = this.getUserById();

        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {

          console.log(errorRes);
          this.isLoading = false;

          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;
        });
  }

  private getUserById(): User {
    return this.dataStorageService.users.find(user => user.id === this.id);
  }

  private initializeId(): void {
    this.route.params.subscribe((params: Params) => this.id = params.id);
  }

  onGoBack(): void {
    if (this.isAdmin) {
      this.helperService.navigateTo('manage-users');
    }
    else {
      this.helperService.navigateTo('account');
    }
  }

  checkUserRole(): void {
    this.isAdmin = this.authService.user.value.isAdmin;
  }
}
