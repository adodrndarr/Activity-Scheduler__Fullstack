import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserToRegisterDTO } from 'src/app/auth/Entities/Models/user.model';
import { AuthService } from '../../../auth/auth.service';
import { RegisterResponseDTO } from '../../../auth/Entities/Interfaces/auth-response';
import { matchPassword } from '../../../auth/register/match.validator';
import { DataStorageService } from '../../../services/data-storage.service';
import { ErrorHandlerService } from '../../../services/error-handler.service';
import { HelperService } from '../../../services/helper.service';
import { ValidatorService } from '../../../services/validator.service';


@Component({
  selector: 'app-register-admin',
  templateUrl: './register-admin.component.html'
})
export class RegisterAdminComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private validatorService: ValidatorService,
    private errorHandlerService: ErrorHandlerService,
    private dataStorageService: DataStorageService,
    private helperService: HelperService
  ) { }


  registerAdminForm: FormGroup;
  registerResponse: RegisterResponseDTO;
  errorMessage: string = null;
  isLoading = false;

  registerAdmin = false;

  ngOnInit(): void {
    this.initializeRegisterForm();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedRegisterForm();
  }


  private initializeRegisterForm(): void {
    this.registerAdminForm = this.formBuilder.group({
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
    if (!this.registerAdminForm.valid) {
      return;
    }

    const user: UserToRegisterDTO = this.registerAdminForm.value;
    const registerObs = this.authService.register(user, this.registerAdmin);

    registerObs.subscribe(
      (responseData) => {
        console.log(responseData);
        this.registerResponse = responseData;
        this.registerAdminForm.reset();

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

  onCheckboxChange(): void {
    this.registerAdmin = !this.registerAdmin;
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.registerAdminForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.registerAdminForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.registerAdminForm);
  }

  public isInvalidMatch(fieldName: string): boolean {
    return this.validatorService.isInvalidMatch(fieldName, this.registerAdminForm);
  }

  public hasInvalidLength(fieldName: string): boolean {
    return this.validatorService.hasInvalidLength(fieldName, this.registerAdminForm);
  }
}
