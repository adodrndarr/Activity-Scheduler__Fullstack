import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ActivityEntity } from 'src/app/auth/Entities/Models/activity.model';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';


@Component({
  selector: 'app-view-activity',
  templateUrl: './view-activity.component.html'
})
export class ViewActivityComponent implements OnInit {
  constructor(
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
    this.getActivityEntity();
  }

  private getActivityEntity(): void {
    this.isLoading = true;
    this.httpService.getActivityEntityById(this.id)
      .subscribe(newActivity => {

        this.activity = newActivity;
        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {

          console.log(errorRes);
          this.isLoading = false;

          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;
        });
  }

  private initializeId(): void {
    this.route.params.subscribe((params: Params) => this.id = params.id);
  }

  onGoBack(): void {
    this.helperService.navigateTo('activities');
  }

  createImagePath(imgPath: any): string {
    if (!imgPath)
      return;

    return this.httpService.createImagePath(imgPath);
  }
}
