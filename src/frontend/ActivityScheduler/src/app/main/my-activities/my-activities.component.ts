import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { Activity } from 'src/app/auth/Entities/Models/activity.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';


@Component({
  selector: 'app-my-activities',
  templateUrl: './my-activities.component.html'
})
export class MyActivitiesComponent implements OnInit, AfterViewChecked {
  constructor(
    private dataStorageService: DataStorageService,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService,
    private authService: AuthService
  ) { }


  activities: Activity[] = [];
  filteredActivities: Activity[] = [];

  errorMessage = null;
  isLoading = true;

  searchTerm: string;
  displayFiltered = false;

  pageItems = [1, 2, 3, 4, 5, 6, 7, 8];
  currentPage = 1;
  totalPages: number;
  totalCount: number;

  ngOnInit() {
    this.initializeActivities();
  }

  ngAfterViewChecked(): void {
    const elems = this.helperService.getPaginationElements();
    this.helperService.markElements(
      elems,
      this.dataStorageService.pagination.currentPage
    );
  }

  initializeActivities() {
    this.updateState();
    this.updateActivities(this.dataStorageService.pagination.currentPage);
  }

  onSearch(): void {
    this.dataStorageService.searchTerm = this.searchTerm;

    if (!this.searchTerm) {
      this.displayFiltered = false;
      this.updateActivities(this.currentPage);

      return;
    }

    this.displayFiltered = true;
    this.currentPage = 1;
    this.updateActivities(this.currentPage);
  }

  onClear(): void {
    this.searchTerm = '';
    this.dataStorageService.searchTerm = this.searchTerm;
    this.displayFiltered = false;

    this.updateActivities(this.currentPage);
  }

  onCancel(activity: Activity): void {
    this.isLoading = true;
    const title = `Cancel event for ${activity.name}?`;

    this.helperService.createCancelAlert(title)
      .then((choice) => {
        if (choice.isConfirmed) {
          this.cancelActivity(activity);
        }
        else {
          this.isLoading = false;
        }
      });
  }

  private cancelActivity(activity: Activity): void {
    this.httpService.cancelActivity(activity.id)
      .subscribe(res => {
        console.log(res);

        this.initializeActivities();
        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);

          this.errorMessage = this.errorHandlerService.errorMessage;
          this.isLoading = false;
        });
  }

  updateActivities(selectedPage: number, value?: string): void {
    this.currentPage = this.helperService
      .markPageAsActive(
        value,
        selectedPage,
        this.currentPage,
        this.dataStorageService.pagination.totalPages
      );

    this.loadActivities(
      this.currentPage,
      this.dataStorageService.pagination.pageSize,
      this.displayFiltered,
      this.searchTerm
    );
  }

  loadActivities(currentPage: number,
                 pageSize: number,
                 showFiltered?: boolean,
                 searchTerm = null
  ): void {
    const user = this.authService.user.value;
    this.isLoading = true;

    this.httpService.getActivities(
      user.id,
      String(currentPage),
      String(pageSize),
      searchTerm
    )
      .subscribe((newActivities: Activity[]) => {
        const totalPages = this.dataStorageService.pagination.totalPages;
        if (totalPages === 0) {
            this.activities.length = 0;
            this.isLoading = false;
            return;
        }

        if (!newActivities.length) {
          this.initializeActivities();
          return;
        }

        console.log(newActivities);
        this.createDate(newActivities);
        this.updateDataStore(newActivities, currentPage);

        if (showFiltered) {
          this.filteredActivities = newActivities;
        }

        this.isLoading = false;
        this.errorMessage = null;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;

          this.filteredActivities.length = 0;
          this.activities.length = 0;

          this.currentPage = 1;
          this.isLoading = false;
        });
  }

  updateState(): void {
    this.searchTerm = this.dataStorageService.searchTerm;
  }

  beautifyDate(date: Date): string {
    const dateMinutes = date.getMinutes();
    const hour = date.getHours();
    const minute = (dateMinutes === 0) ? `${dateMinutes}0` : `${dateMinutes}`;

    const fullInfo = `${hour}:${minute}h`;
    return fullInfo;
  }

  createDate(activities: Activity[]): any {
    activities.forEach(a => {
      a.bookedForDate = new Date(a.bookedForDate);
      a.startTime = new Date(a.startTime);
      a.endTime = new Date(a.endTime);
    });

    return activities;
  }

  updateDataStore(activities: Activity[], currentPage: number): void {
    const dataStore = this.dataStorageService;

    this.activities = activities;
    dataStore.activities = activities;

    dataStore.pagination.currentPage = currentPage;
    this.totalPages = dataStore.pagination.totalPages;
    this.totalCount = dataStore.pagination.totalCount;
  }
}
