import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activity.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import { ValidatorService } from '../../../services/validator.service';


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
    private helperService: HelperService,
    private dataStorageService: DataStorageService
  ) { }


  newActivityForm: FormGroup;
  isLoading = false;
  errorMessage: string = null;

  ngOnInit() {
    this.initializeNewActivityForm();
  }

  private initializeNewActivityForm(): void {
    this.newActivityForm = this.formBuilder.group({
      'name': [null, [
          Validators.required,
          Validators.maxLength(20),
          Validators.pattern('^[A-Z]+.*$')
        ]
      ],
      'imageUrl': [null, Validators.required],
      'itemQuantity': [null, [
          Validators.required,
          Validators.pattern(`^[-+]?\\d+$`),
          Validators.maxLength(4)
        ]
      ],
      'minUserCount': [null, [
          Validators.required,
          Validators.pattern('^[-+]?\\d+$'),
          Validators.maxLength(4)
        ]
      ],
      'maxUserCount': [null, [
          Validators.required,
          Validators.pattern('^[-+]?\\d+$'),
          Validators.maxLength(4)
        ]
      ],
      'description': [null, [
          Validators.required,
          Validators.maxLength(400)
        ]
      ],
      'location': [null, [
          Validators.required,
          Validators.maxLength(100)
        ]
      ]
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
        this.resetActivityEntities();

        this.helperService.navigateTo('activities');
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

  private resetState(): void {
    this.errorMessage = null;
  }

  resetActivityEntities(): void {
    this.dataStorageService.activityEntities.length = 0;
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

  public hasInvalidLength(fieldName: string): boolean {
    return this.validatorService.hasInvalidLength(fieldName, this.newActivityForm);
  }
}

