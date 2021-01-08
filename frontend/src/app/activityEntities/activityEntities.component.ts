import { Component, OnInit } from '@angular/core';
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
    private httpService: HttpService
  ) { }


  activityEntities: ActivityEntity[] = [];
  error = null;
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
        (errorRes) => {
          this.error = errorRes;
          this.isLoading = false;
        });
    }
    else {
      this.activityEntities = this.dataStorageService.activityEntities;
      this.isLoading = false;
    }
  }
}
