import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { ActivityEntity } from '../auth/Entities/Models/activityEntity.model';
import { DataStorageService } from '../services/data-storage.service';
import { HttpService } from '../services/http.service';


@Component({
  selector: 'app-activities',
  templateUrl: './activityEntities.component.html'
})
export class ActivityEntitiesComponent implements OnInit {

  constructor(
    private dataStorageService: DataStorageService,
    private httpService: HttpService,
    private authService: AuthService
  ) { }


  activityEntities: ActivityEntity[] = [];
  errorMessage = null;
  isLoading = true;

  ngOnInit() {
    this.initializeActivityEntities();
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
          this.authService.handleError(errorRes);

          this.errorMessage = this.authService.errorMessage;
          this.isLoading = false;
        });
    }
    else {
      this.activityEntities = this.dataStorageService.activityEntities;
      this.isLoading = false;
    }
  }
}
