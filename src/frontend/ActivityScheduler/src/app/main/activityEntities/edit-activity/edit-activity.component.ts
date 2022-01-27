import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { Params } from '@angular/router';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activity.model';
import * as constants from '../../../shared/constants';
import { BaseActivityEntityComponent } from '../common/base-activity-entity/base-activity-entity.component';

@Component({
  selector: 'app-edit-activity',
  templateUrl: './edit-activity.component.html'
})
export class EditActivityComponent extends BaseActivityEntityComponent implements OnInit, OnDestroy {

  editActivityForm: FormGroup;
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
    this.populateEditActivityForm({} as ActivityEntity);
    if (!this.activity) {
      this.getActivityEntity();
      return;
    }

    this.populateEditActivityForm(this.activity);
  }

  private getActivityEntity(): void {
    this.isLoading = true;
    this.httpService.getActivityEntityById(this.id)
      .subscribe(newActivity => {

        this.activity = newActivity;
        this.populateEditActivityForm(this.activity);
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

    this.uploadFile();
    this.uploadSub = this.uploadFinished.subscribe(uploadFinished => {
      if (uploadFinished) {
        const updatedctivityEntity: ActivityEntity = this.editActivityForm.value;

        updatedctivityEntity.itemQuantity = +updatedctivityEntity.itemQuantity;
        updatedctivityEntity.minUserCount = +updatedctivityEntity.minUserCount;
        updatedctivityEntity.maxUserCount = +updatedctivityEntity.maxUserCount;
        updatedctivityEntity.imagePath = this.dataStorageService.currentImagePath;

        this.editActivityEntity(updatedctivityEntity);
      }
    });

  }

  private editActivityEntity(updatedctivityEntity: ActivityEntity) {
    const updateObs = this.httpService.editActivityEntity(this.activity.id, updatedctivityEntity);
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
    this.route.params.subscribe((params: Params) => this.id = params.id);
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

  private populateEditActivityForm(activityEntity: ActivityEntity): void {
    this.editActivityForm = this.formBuilder.group({
      'name': [activityEntity.name, [
        Validators.required,
        Validators.maxLength(20)
      ]],
      // 'imageUrl': [imageUrl, Validators.required],
      'itemQuantity': [activityEntity.itemQuantity, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'minUserCount': [activityEntity.minUserCount, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'maxUserCount': [activityEntity.maxUserCount, [
        Validators.required,
        Validators.pattern(constants.MATCH_NUMBERS),
        Validators.maxLength(4)
      ]],
      'description': [activityEntity.description, [
        Validators.required,
        Validators.maxLength(400)
      ]],
      'location': [activityEntity.location, [
        Validators.required,
        Validators.maxLength(100)
      ]]
    });
  }

  ngOnDestroy(): void {
    if (this.uploadSub)
      this.uploadSub.unsubscribe();
  }
}
