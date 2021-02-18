import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activity.model';
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
    private dataStorageService: DataStorageService,
    private route: ActivatedRoute
  ) { }


  editActivityForm: FormGroup;
  isLoading = false;
  errorMessage: string = null;
  activity: ActivityEntity;
  id: string;

  ngOnInit() {
    this.initializeId();
    this.activity = this.getActivityById(this.id);
    this.initializeEditActivityForm();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedEditActivityForm();
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

    if (!this.activity) {
      this.getActivityEntity();
      return;
    }

    this.populateEditActivityForm({
      name: this.activity.name,
      imageUrl: this.activity.imageUrl,
      itemQuantity: this.activity.itemQuantity,
      minUserCount: this.activity.minUserCount,
      maxUserCount: this.activity.maxUserCount,
      description: this.activity.description,
      location: this.activity.location
    });
  }

  private getActivityEntity(): void {
    this.isLoading = true;
    this.httpService.getActivityEntityById(this.id)
      .subscribe(newActivity => {
        this.activity = newActivity;

        this.populateEditActivityForm({
          name: this.activity.name,
          imageUrl: this.activity.imageUrl,
          itemQuantity: this.activity.itemQuantity,
          minUserCount: this.activity.minUserCount,
          maxUserCount: this.activity.maxUserCount,
          description: this.activity.description,
          location: this.activity.location
        });

        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.isLoading = false;

          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;
        });
  }

  private handleSubmittedEditActivityForm(): void {
    if (!this.editActivityForm.valid) {
      return;
    }

    const updatedctivityEntity: ActivityEntity = this.editActivityForm.value;

    updatedctivityEntity.itemQuantity = +updatedctivityEntity.itemQuantity;
    updatedctivityEntity.minUserCount = +updatedctivityEntity.minUserCount;
    updatedctivityEntity.maxUserCount = +updatedctivityEntity.maxUserCount;

    const updateObs = this.httpService.editActivityEntity
      (
        this.activity.id,
        updatedctivityEntity
      );

    updateObs.subscribe(
      (updateResponse) => {
        console.log(updateResponse);

        this.editActivityForm.reset();
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

  private initializeId(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = params.id;
        }
      );
  }

  private getActivityById(id: string): ActivityEntity {
    return this.dataStorageService.activityEntities
      .find(activityEntity => activityEntity.id === id);
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
      'name': [name, [
        Validators.required,
        Validators.maxLength(20)
      ]
      ],
      'imageUrl': [imageUrl, Validators.required],
      'itemQuantity': [itemQuantity, [
        Validators.required,
        Validators.pattern(`^[-+]?\\d+$`),
        Validators.maxLength(4)
      ]
      ],
      'minUserCount': [minUserCount, [
        Validators.required,
        Validators.pattern('^[-+]?\\d+$'),
        Validators.maxLength(4)
      ]
      ],
      'maxUserCount': [maxUserCount, [
        Validators.required,
        Validators.pattern('^[-+]?\\d+$'),
        Validators.maxLength(4)
      ]
      ],
      'description': [description, [
        Validators.required,
        Validators.maxLength(400)
      ]
      ],
      'location': [location, [
        Validators.required,
        Validators.maxLength(100)
      ]
      ]
    });
  }

  resetActivityEntities(): void {
    this.dataStorageService.activityEntities.length = 0;
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

  public hasInvalidLength(fieldName: string): boolean {
    return this.validatorService.hasInvalidLength(fieldName, this.editActivityForm);
  }

}
