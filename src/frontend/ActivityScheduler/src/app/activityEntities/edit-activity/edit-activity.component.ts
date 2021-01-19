import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activityEntity.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import { ValidatorService } from 'src/app/services/validator.service';


@Component({
  selector: 'app-edit-activity',
  templateUrl: './edit-activity.component.html'
})
export class EditActivityComponent implements OnInit {
  constructor(
    private validatorService: ValidatorService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService,
    private dataStorageService: DataStorageService
  ) { }


  editActivityForm: FormGroup;
  isLoading = false;
  errorMessage: string = null;
  selectedActivity: ActivityEntity;

  ngOnInit() {
    this.selectedActivity = this.dataStorageService.selectedActivity;
    this.initializeEditActivityForm();
  }

  private initializeEditActivityForm(): void {
    this.populateEditActivityForm({
      name: null,
      imageUrl: null,
      itemQuantity: null,
      minUserCount: null,
      maxUserCount: null,
      description: null,
      location: null
    });

    if (!this.selectedActivity) {
      this.helperService.navigateAndUpdateTo('activities');
      return;
    }

    this.populateEditActivityForm({
      name: this.selectedActivity.name,
      imageUrl: this.selectedActivity.imageUrl,
      itemQuantity: this.selectedActivity.itemQuantity,
      minUserCount: this.selectedActivity.minUserCount,
      maxUserCount: this.selectedActivity.maxUserCount,
      description: this.selectedActivity.description,
      location: this.selectedActivity.location
    });
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedNewActivityForm();
  }

  private handleSubmittedNewActivityForm(): void {
    if (!this.editActivityForm.valid) {
      return;
    }

    const updatedctivityEntity: ActivityEntity = this.editActivityForm.value;

    updatedctivityEntity.id = this.selectedActivity.id;
    updatedctivityEntity.itemQuantity = +updatedctivityEntity.itemQuantity;
    updatedctivityEntity.minUserCount = +updatedctivityEntity.minUserCount;
    updatedctivityEntity.maxUserCount = +updatedctivityEntity.maxUserCount;

    const updateObs = this.httpService.editActivityEntity(updatedctivityEntity.id, updatedctivityEntity);
    updateObs.subscribe(
      (updateResponse) => {
        console.log(updateResponse);

        this.editActivityForm.reset();
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

  private resetState(): void {
    this.errorMessage = null;
  }

  private populateEditActivityForm(
    {
      name,
      imageUrl,
      itemQuantity,
      minUserCount,
      maxUserCount,
      description,
      location
    }
  ): void {
    this.editActivityForm = this.formBuilder.group({
      'name': [name, Validators.required],
      'imageUrl': [imageUrl, Validators.required],
      'itemQuantity': [itemQuantity, [
        Validators.required,
        Validators.pattern(`^[-+]?\\d+$`)
      ]
      ],
      'minUserCount': [minUserCount, [
        Validators.required,
        Validators.pattern('^[-+]?\\d+$')
      ]
      ],
      'maxUserCount': [maxUserCount, [
        Validators.required,
        Validators.pattern('^[-+]?\\d+$')
      ]
      ],
      'description': [description, Validators.required],
      'location': [location, Validators.required]
    });
  }

  public isInvalidInput(fieldName: string): boolean {
    return this.validatorService.isInvalidInput(fieldName, this.editActivityForm);
  }

  public isRequired(fieldName: string): boolean {
    return this.validatorService.isRequired(fieldName, this.editActivityForm);
  }

  public isInvalidFormat(fieldName: string): boolean {
    return this.validatorService.isInvalidFormat(fieldName, this.editActivityForm);
  }

}
