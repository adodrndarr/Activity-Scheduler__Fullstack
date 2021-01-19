import { Component, Input, OnInit } from '@angular/core';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activityEntity.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { HelperService } from 'src/app/services/helper.service';


@Component({
  selector: 'app-view-activity',
  templateUrl: './view-activity.component.html'
})
export class ViewActivityComponent implements OnInit {
  constructor(
    private dataStorageService: DataStorageService,
    private helperService: HelperService
  ) { }


  activity: ActivityEntity;

  ngOnInit() {
    this.activity = this.dataStorageService.selectedActivity;
    if (!this.activity) {
      this.helperService.navigateTo('activities');
    }
  }

  onGoBack(): void {
    this.helperService.navigateTo('activities');
  }

}
