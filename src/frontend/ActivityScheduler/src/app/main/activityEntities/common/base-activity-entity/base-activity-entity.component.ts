import { HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import { ValidatorService } from 'src/app/services/validator.service';

@Component({
  selector: 'app-base-activity-entity',
  template: ``
})
export abstract class BaseActivityEntityComponent {

  constructor(
    protected validatorService: ValidatorService,
    protected formBuilder: FormBuilder,
    protected httpService: HttpService,
    protected errorHandlerService: ErrorHandlerService,
    protected helperService: HelperService,
    protected dataStorageService: DataStorageService,
    protected route: ActivatedRoute
  ) { }

  uploadFinished = new BehaviorSubject<boolean>(false);
  fileToUpload: File;
  isLoading = false;
  progress: number;

  uploadMessage: string;
  uploadResponse: any;
  uploadSub: Subscription;

  saveFileForUpload(file): void {
    const files = file.files;
    if (files.length === 0) {
      this.progress = 0;
      this.uploadMessage = null;
      return;
    }

    this.fileToUpload = <File>files[0];
  }

  protected resetActivityEntities(): void {
    this.dataStorageService.activityEntities.length = 0;
    this.dataStorageService.currentImagePath = null;
  }

  protected uploadFile(): void {
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

  isInvalidInput(fieldName: string, form: FormGroup): boolean {
    return this.validatorService.isInvalidInput(fieldName, form);
  }

  isRequired(fieldName: string, form: FormGroup): boolean {
    return this.validatorService.isRequired(fieldName, form);
  }

  isInvalidFormat(fieldName: string, form: FormGroup): boolean {
    return this.validatorService.isInvalidFormat(fieldName, form);
  }

  hasInvalidLength(fieldName: string, form: FormGroup): boolean {
    return this.validatorService.hasInvalidLength(fieldName, form);
  }
}
