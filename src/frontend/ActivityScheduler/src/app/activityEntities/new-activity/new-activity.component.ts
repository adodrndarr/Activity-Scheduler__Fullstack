import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activityEntity.model';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import { ValidatorService } from '../../services/validator.service';


@Component({
  selector: 'app-new-activity',
  templateUrl: './new-activity.component.html'
})
export class NewActivityComponent implements OnInit {
  constructor(
    private validatorService: ValidatorService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService
  ) { }


  newActivityForm: FormGroup;
  isLoading = false;
  errorMessage: string = null;

  ngOnInit() {
    this.initializeNewActivityForm();
  }

  private initializeNewActivityForm(): void {
    this.newActivityForm = this.formBuilder.group({
      'name': [null, Validators.required],
      'imageUrl': [null, Validators.required],
      'itemQuantity': [null, [
          Validators.required,
          Validators.pattern(`^[-+]?\\d+$`)
        ]
      ],
      'minUserCount': [null, [
          Validators.required,
          Validators.pattern('^[-+]?\\d+$')
        ]
      ],
      'maxUserCount': [null, [
          Validators.required,
          Validators.pattern('^[-+]?\\d+$')
        ]
      ],
      'description': [null, Validators.required],
      'location': [null, Validators.required]
    });
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedNewActivityForm();
  }

  private handleSubmittedNewActivityForm(): void {
    if (!this.newActivityForm.valid) {
      return;
    }

    const activityEntity: ActivityEntity = this.newActivityForm.value;

    activityEntity.itemQuantity = +activityEntity.itemQuantity;
    activityEntity.minUserCount = +activityEntity.minUserCount;
    activityEntity.maxUserCount = +activityEntity.maxUserCount;

    const creationObs = this.httpService.createActivityEntity(activityEntity);
    creationObs.subscribe(
      (creationResponse) => {
        console.log(creationResponse);

        this.newActivityForm.reset();
        this.helperService.navigateAndUpdateTo('activities');
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

  onCancel(): void {
    this.helperService.navigateTo('activities');
  }

  // loadImage(event): void {
  //   // const image: any = document.getElementById('outputImg');
  //   const imgUrlInput: any = document.getElementById('imageUrl');
  //   const imgSrc = URL.createObjectURL(event.target.files[0]);
  //   // image.src = URL.createObjectURL(event.target.files[0]);
  //   imgUrlInput.value = '';
  //   imgUrlInput.value = imgSrc;
  // }

  private resetState(): void {
    this.errorMessage = null;
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.newActivityForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.newActivityForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.newActivityForm);
  }

}

