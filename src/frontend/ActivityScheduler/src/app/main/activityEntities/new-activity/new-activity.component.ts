import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activity.model';
import * as constants from '../../../shared/constants';
import { BaseActivityEntityComponent } from '../common/base-activity-entity/base-activity-entity.component';

@Component({
  selector: 'app-new-activity',
  templateUrl: './new-activity.component.html'
})
export class NewActivityComponent extends BaseActivityEntityComponent implements OnInit, OnDestroy {

  newActivityForm: FormGroup;
  errorMessage: string = null;

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
        Validators.pattern(constants.MATCH_FIRST_LETTER_CAPITAL)
      ]],
      'itemQuantity': [null, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'minUserCount': [null, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'maxUserCount': [null, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'description': [null, [
        Validators.required,
        Validators.maxLength(400)
      ]],
      'location': [null, [
        Validators.required,
        Validators.maxLength(100)
      ]]
    });
  }

  private handleSubmittedNewActivityForm(): void {
    if (!this.newActivityForm.valid) {
      return;
    }

    this.uploadFile();
    this.uploadSub = this.uploadFinished.subscribe(uploadFinished => {
      if (uploadFinished) {
        const activityEntity: ActivityEntity = this.newActivityForm.value;

        activityEntity.itemQuantity = +activityEntity.itemQuantity;
        activityEntity.minUserCount = +activityEntity.minUserCount;
        activityEntity.maxUserCount = +activityEntity.maxUserCount;
        activityEntity.imagePath = this.dataStorageService.currentImagePath;

        this.createActivityEntity(activityEntity);
      }
    });
  }

  private createActivityEntity(activityEntity: ActivityEntity) {
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

  ngOnDestroy(): void {
    if (this.uploadSub)
      this.uploadSub.unsubscribe();
  }
}

