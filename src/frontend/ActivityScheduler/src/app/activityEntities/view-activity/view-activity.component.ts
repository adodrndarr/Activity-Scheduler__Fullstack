import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activityEntity.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';


@Component({
  selector: 'app-view-activity',
  templateUrl: './view-activity.component.html'
})
export class ViewActivityComponent implements OnInit {
  constructor(
    private dataStorageService: DataStorageService,
    private helperService: HelperService,
    private route: ActivatedRoute,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService
  ) { }


  activity: ActivityEntity;
  id: string;
  errorMessage: string;
  isLoading = false;

  ngOnInit() {
    this.initializeId();
    this.activity = this.getActivityById();
    if (!this.activity) {
      this.getActivityEntities();
    }
  }

  private getActivityEntities(): void {
    this.isLoading = true;
    this.httpService.getActivityEntities()
      .subscribe(newActivities => {
        this.dataStorageService.activityEntities = newActivities;
        this.activity = this.getActivityById();

        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.isLoading = false;

          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;
        });
  }

  private getActivityById(): ActivityEntity {
    return this.dataStorageService.activityEntities
      .find(activityEntity => activityEntity.id === this.id);
  }

  private initializeId(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = params.id;
        }
      );
  }

  onGoBack(): void {
    this.helperService.navigateTo('activities');
  }

}
