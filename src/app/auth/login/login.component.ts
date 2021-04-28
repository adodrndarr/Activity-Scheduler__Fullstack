import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { ValidatorService } from '../../services/validator.service';
import { AuthService } from '../auth.service';
import { LoginResponseDTO } from '../Entities/Interfaces/auth-response';
import { UserToLoginDTO } from '../Entities/Models/user.model';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private validatorService: ValidatorService,
    private errorHandlerService: ErrorHandlerService
  ) { }


  loginForm: FormGroup;
  loginResponse: LoginResponseDTO;
  errorMessage: string = null;
  isLoading = false;

  ngOnInit(): void {
    this.initializeLoginForm();
  }

  private initializeLoginForm(): void {
    this.loginForm = this.formBuilder.group({
      'email': [null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$')
        ]
      ],
      'password': [null, Validators.required]
    });
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedLoginForm();
  }

  private handleSubmittedLoginForm(): void {
    if (!this.loginForm.valid) {
      return;
    }

    const user: UserToLoginDTO = this.loginForm.value;
    const loginObs = this.authService.login(user);

    loginObs.subscribe(
      (loginResponse) => {
        console.log(loginResponse);
        this.authService.handleLoginResponse(loginResponse);
        this.loginResponse = loginResponse;

        this.loginForm.reset();
        this.router.navigateByUrl('/activities');
        this.isLoading = false;
      },
      (errorResponse: HttpErrorResponse) => {
        console.log(errorResponse);
        this.errorHandlerService.handleError(errorResponse);

        this.loginResponse = errorResponse.error;
        this.errorMessage = this.errorHandlerService.errorMessage;

        this.loginForm.controls['password'].reset();
        this.isLoading = false;
      }
    );
  }

  private resetState(): void {
    this.errorMessage = null;
    this.loginResponse = null;
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.loginForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.loginForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.loginForm);
  }
}

