import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { ValidatorService } from 'src/app/services/validator.service';
import { AuthService } from '../auth.service';
import { RegisterResponseDTO } from '../Entities/Interfaces/auth-response';
import { UserToRegisterDTO } from '../Entities/Models/user.model';
import { matchPassword } from './match.validator';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private validatorService: ValidatorService,
    private errorHandlerService: ErrorHandlerService,
    private dataStorageService: DataStorageService,
    private helperService: HelperService
  ) { }


  registerForm: FormGroup;
  registerResponse: RegisterResponseDTO;
  errorMessage: string = null;
  isLoading = false;

  @Input() registerAdmin: boolean;

  ngOnInit(): void {
    this.initializeRegisterForm();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedRegisterForm();
  }


  private initializeRegisterForm(): void {
    this.registerForm = this.formBuilder.group({
      'userName': [null, [
        Validators.required,
        Validators.pattern('^[A-Z]+.*$'),
        Validators.maxLength(20)
      ]],
      'lastName': [null, [
        Validators.required,
        Validators.pattern('^[A-Z]+.*$'),
        Validators.maxLength(20)
      ]],
      'email': [null, [
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$'),
        Validators.maxLength(20)
        ]
      ],
      'password': [null,
        [
          Validators.required,
          Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{6,}$')
        ]
      ],
      'confirmPassword': [null, Validators.required]
    }, { validator: matchPassword });
  }

  private handleSubmittedRegisterForm(): void {
    if (!this.registerForm.valid) {
      return;
    }

    const user: UserToRegisterDTO = this.registerForm.value;
    const registerObs = this.registerAdmin ?
      this.authService.register(user, true) :
      this.authService.register(user);

    registerObs.subscribe(
      (responseData) => {
        console.log(responseData);
        this.registerResponse = responseData;

        this.registerForm.reset();
        this.resetUsers();

        this.helperService.navigateTo('manage-users');
        this.isLoading = false;
      },
      (errorResponse: HttpErrorResponse) => {
        console.log(errorResponse);
        this.errorHandlerService.handleError(errorResponse);

        this.registerResponse = errorResponse.error;
        this.errorMessage = this.errorHandlerService.errorMessage;

        this.isLoading = false;
      }
    );
  }

  onCancel(): void {
    this.helperService.navigateTo('manage-users');
  }

  private resetState(): void {
    this.errorMessage = null;
    this.registerResponse = null;
  }

  resetUsers(): void {
    this.dataStorageService.users.length = 0;
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.registerForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.registerForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.registerForm);
  }

  public isInvalidMatch(fieldName: string): boolean {
    return this.validatorService.isInvalidMatch(fieldName, this.registerForm);
  }

  public hasInvalidLength(fieldName: string): boolean {
    return this.validatorService.hasInvalidLength(fieldName, this.registerForm);
  }
}
