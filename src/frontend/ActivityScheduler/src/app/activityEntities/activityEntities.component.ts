import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { ActivityEntity } from '../auth/Entities/Models/activityEntity.model';
import { DataStorageService } from '../services/data-storage.service';
import { ErrorHandlerService } from '../services/error-handler.service';
import { HelperService } from '../services/helper.service';
import { HttpService } from '../services/http.service';


@Component({
  selector: 'app-activities',
  templateUrl: './activityEntities.component.html'
})
export class ActivityEntitiesComponent implements OnInit, OnDestroy {

  constructor(
    private dataStorageService: DataStorageService,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService,
    private authService: AuthService
  ) { }


  userSub: Subscription;

  activityEntities: ActivityEntity[] = [];
  filteredActivityEntities: ActivityEntity[] = [];
  displayFiltered = false;

  errorMessage = null;
  isLoading = true;
  searchTerm: string;
  isAdmin = false;


  ngOnInit() {
    this.initializeActivityEntities();
    this.checkUserRole();
  }

  initializeActivityEntities() {
    const haveNoActivities = this.dataStorageService.activityEntities.length === 0;
    if (haveNoActivities) {
      this.httpService.getActivityEntities()
        .subscribe(newActivities => {
          this.activityEntities = newActivities;
          this.dataStorageService.activityEntities = newActivities;
          this.isLoading = false;
        },
          (errorRes: HttpErrorResponse) => {
            console.log(errorRes);
            this.errorHandlerService.handleError(errorRes);

            this.errorMessage = this.errorHandlerService.errorMessage;
            this.isLoading = false;
          });
    }
    else {
      this.activityEntities = this.dataStorageService.activityEntities;
      this.isLoading = false;
    }
  }

  private checkUserRole(): void {
    this.userSub = this.authService.user
    .subscribe(user => {
      if (user) {
        this.isAdmin = user.isAdmin;
      }
      else {
        this.isAdmin = false;
      }
    });
  }

  onSearch(): void {
    const searchName: string = this.searchTerm.toLowerCase();

    this.filteredActivityEntities = this.activityEntities
      .filter(activityEntity => {
          const activityName = activityEntity.name.toLowerCase();

          return activityName.includes(searchName);
        }
      );

    if (!searchName) {
      this.displayFiltered = false;
    }

    this.displayFiltered = true;
  }

  onClear(): void {
    this.searchTerm = '';
    this.displayFiltered = false;
  }

  onSelect(activity: ActivityEntity): void {
    this.dataStorageService.selectedActivity = activity;
  }

  onDelete(activity: ActivityEntity): void {
    if (!this.isAdmin) {
      return;
    }

    this.isLoading = true;
    const title = `Delete ${activity.name}?`;

    this.helperService.createAlert(title, activity.imageUrl)
      .then((choice) => {
        if (choice.isConfirmed) {
          this.deleteActivity(activity);
        }
        else {
          this.isLoading = false;
        }
      });
  }

  private deleteActivity(activity: ActivityEntity): void {
    this.httpService.deleteActivityEntity(activity.id)
      .subscribe(res => {
        console.log(res);

        this.helperService.navigateAndUpdateTo('activities');
        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);

          this.errorMessage = this.errorHandlerService.errorMessage;
          this.isLoading = false;
        });
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

}
