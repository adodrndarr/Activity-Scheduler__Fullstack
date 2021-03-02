import { HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
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

  uploadMessage: string;
  progress: number;
  uploadResponse: any;
  fileToUpload: File;
  uploadFinished = new BehaviorSubject<boolean>(false);

  ngOnInit() {
    this.initializeNewActivityForm();
  }

  onSubmit(): void {
    this.isLoading = true;
    this.resetState();
    this.handleSubmittedNewActivityForm();
  }

  private initializeNewActivityForm(): void {
    this.newActivityForm = this.formBuilder.group({
      'name': [null, [
        Validators.required,
        Validators.maxLength(20),
        Validators.pattern('^[A-Z]+.*$')
      ]
      ],
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

  saveFileForUpload(file): void {
    const files = file.files;
    if (files.length === 0) {
      this.progress = 0;
      this.uploadMessage = null;
      return;
    }

    this.fileToUpload = <File>files[0];
  }

  uploadFile(): void {
    if (!this.fileToUpload) {
      this.uploadFinished.next(true);
      return;
    }

    const formData = new FormData();
    formData.append('file', this.fileToUpload, this.fileToUpload.name);

    this.isLoading = true;
    this.httpService.uploadFile(formData)
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          const uploadProgress = (event.loaded / event.total) * 100;
          this.progress = Math.round(uploadProgress);
        }
        else if (event.type === HttpEventType.Response) {
          this.uploadMessage = 'Upload finished.';
          this.uploadResponse = event.body;

          console.log(this.uploadResponse);
          this.dataStorageService.currentImagePath = this.uploadResponse.serverFilePath;

          this.uploadFinished.next(true);            
          this.isLoading = false;         
        }
      });
  }

  private handleSubmittedNewActivityForm(): void {
    if (!this.newActivityForm.valid) {
      return;
    }

    this.uploadFile();
    this.uploadFinished.subscribe(uploadFinished => {
      if (uploadFinished) {
        const activityEntity: ActivityEntity = this.newActivityForm.value;

        activityEntity.itemQuantity = +activityEntity.itemQuantity;
        activityEntity.minUserCount = +activityEntity.minUserCount;
        activityEntity.maxUserCount = +activityEntity.maxUserCount;
        activityEntity.imagePath = this.dataStorageService.currentImagePath;

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
    });
  }

  onCancel(): void {
    this.helperService.navigateTo('activities');
  }

  private resetState(): void {
    this.errorMessage = null;
  }

  resetActivityEntities(): void {
    this.dataStorageService.activityEntities.length = 0;
    this.dataStorageService.currentImagePath = null;
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

