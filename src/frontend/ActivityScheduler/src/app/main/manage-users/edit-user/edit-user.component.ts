import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/Entities/Models/user.model';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import { ValidatorService } from 'src/app/services/validator.service';


@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html'
})
export class EditUserComponent implements OnInit {
  constructor(
    private validatorService: ValidatorService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService,
    private route: ActivatedRoute,
    private authService: AuthService
  ) { }


  editUserForm: FormGroup;
  isLoading = false;
  errorMessage: string = null;
  user: User;
  id: string;
  isAdmin = false;

  ngOnInit() {
    this.initializeId();
    this.user = this.getCurrentUser();
    this.checkUserRole();
    this.initializeEditUserForm();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedEditUserForm();
  }

  private initializeEditUserForm(): void {
    this.populateEditUserForm({
      name: null,
      lastName: null,
      email: null,
      isAdmin: false
    });

    if (!this.user) {
      this.getUser();
      return;
    }

    this.populateEditUserForm({
      name: this.user.userName,
      lastName: this.user.lastName,
      email: this.user.email,
      isAdmin: this.user.isAdmin
    });
  }

  private getUser(): void {
    this.isLoading = true;

    this.httpService.getUserById(this.id)
      .subscribe(newUser => {
        this.user = newUser;

        if (this.user) {
          this.populateEditUserForm({
            name: this.user.userName,
            lastName: this.user.lastName,
            email: this.user.email,
            isAdmin: this.user.isAdmin
          });

          this.isLoading = false;
          return;
        }

        this.helperService.navigateTo('not-found');
      },
      (errorRes: HttpErrorResponse) => {
        console.log(errorRes);
        this.isLoading = false;

        this.errorHandlerService.handleError(errorRes);
        this.errorMessage = this.errorHandlerService.errorMessage;
      });
  }

  private handleSubmittedEditUserForm(): void {
    if (!this.editUserForm.valid) {
      return;
    }

    const updatedUser: User = this.editUserForm.value;
    updatedUser.normalizedUserName = updatedUser.userName.toUpperCase();
    updatedUser.normalizedEmail = updatedUser.email.toUpperCase();

    const updateObs = this.httpService.editUser(this.user.id, updatedUser);

    updateObs.subscribe(
      (updateResponse) => {
        console.log(updateResponse);

        const updateCurrentUser = this.authService.user.value.id === this.user.id;
        if (updateCurrentUser) {
          updatedUser.id = this.user.id;
          this.authService.user.next(updatedUser);
        }

        this.checkUserAndNavigate();

        this.editUserForm.reset();
        this.isLoading = false;
      },
      (errorResponse: HttpErrorResponse) => {
        console.log(errorResponse);

        this.errorHandlerService.handleError(errorResponse);
        this.errorMessage = this.errorHandlerService.errorMessage;

        this.isLoading = false;
      }
    );
  }

  private initializeId(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = params.id;
        }
      );
  }

  private getCurrentUser(): User {
    return this.authService.user.value;
  }

  onCancel(): void {
    this.checkUserAndNavigate();
  }

  private resetState(): void {
    this.errorMessage = null;
  }

  private populateEditUserForm(
    {
      name,
      lastName,
      email,
      isAdmin
    }
  ): void {
    this.editUserForm = this.formBuilder.group({
      'userName': [name, [
        Validators.required,
        Validators.pattern('^[A-Z]+.*$'),
        Validators.maxLength(20)
      ]],
      'lastName': [lastName, [
        Validators.required,
        Validators.pattern('^[A-Z]+.*$'),
        Validators.maxLength(20)
      ]],
      'email': [email, [
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9.!#$%&’*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$'),
        Validators.maxLength(20)
      ]
      ],
      'isAdmin': [isAdmin]
    });
  }

  checkUserRole(): void {
    this.isAdmin = this.authService.user.value.isAdmin;
    const updateCurrentUser = this.authService.user.value.id === this.id;

    if (!this.isAdmin && !updateCurrentUser) {
      this.helperService.navigateTo('not-found');
    }
  }

  checkUserAndNavigate(): void {
    if (this.isAdmin) {
      this.helperService.navigateTo('manage-users');
    }
    else {
      this.helperService.navigateTo('account');
    }
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.editUserForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.editUserForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.editUserForm);
  }

  public hasInvalidLength(fieldName: string): boolean {
    return this.validatorService.hasInvalidLength(fieldName, this.editUserForm);
  }
}
