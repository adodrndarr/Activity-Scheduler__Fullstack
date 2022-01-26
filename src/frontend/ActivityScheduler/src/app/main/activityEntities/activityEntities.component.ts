import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { PaginationInfo } from 'src/app/auth/Entities/Models/pagination.model';
import { PaginationRequest } from 'src/app/auth/Entities/Models/requests/pagination-request.model';
import { AuthService } from '../../auth/auth.service';
import { ActivityEntity } from '../../auth/Entities/Models/activity.model';
import { DataStorageService } from '../../services/data-storage.service';
import { ErrorHandlerService } from '../../services/error-handler.service';
import { HelperService } from '../../services/helper.service';
import { HttpService } from '../../services/http.service';


@Component({
  selector: 'app-activities',
  templateUrl: './activityEntities.component.html'
})
export class ActivityEntitiesComponent implements OnInit, AfterViewChecked, OnDestroy {

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
  paginationRequest: PaginationRequest = {};
  displayFiltered = false;

  errorMessage = null;
  isLoading = true;
  isAdmin = false;
  searchTerm: string;

  pageItems = [1, 2, 3, 4, 5, 6, 7, 8];
  currentPage = 1;
  totalPages: number;
  totalCount: number;

  ngOnInit() {
    this.initializeActivityEntities();
    this.checkUserRole();
  }

  ngAfterViewChecked(): void {
    const elems = this.helperService.getPaginationElements();
    this.helperService.markElements(
      elems,
      this.dataStorageService.pagination.currentPage
    );
  }

  initializeActivityEntities() {
    this.updateState();
    this.updateActivities(this.dataStorageService.pagination.currentPage);
  }

  private checkUserRole(): void {
    this.userSub = this.authService.user
      .subscribe(user => {

        if (user)
          this.isAdmin = user.isAdmin;
        else
          this.isAdmin = false;
      });
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

  onDelete(activity: ActivityEntity): void {
    if (!this.isAdmin) {
      return;
    }

    this.isLoading = true;
    const title = `Delete ${activity.name}?`;

    this.helperService.createAlert(title, this.createImagePath(activity.imagePath))
      .then((choice) => {

        if (choice.isConfirmed)
          this.deleteActivity(activity);
        else
          this.isLoading = false;
      });
  }

  private deleteActivity(activity: ActivityEntity): void {
    this.httpService.deleteActivityEntity(activity.id)
      .subscribe(res => {
        console.log(res);

        this.initializeActivityEntities();
        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {

          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);

          this.errorMessage = this.errorHandlerService.errorMessage;
          this.isLoading = false;
        });
  }

  updateState(): void {
    this.searchTerm = this.dataStorageService.searchTerm;
  }

  updateActivities(selectedPage: number, value?: string): void {
    const paginationInfo = new PaginationInfo(
      value,
      selectedPage,
      this.currentPage,
      this.dataStorageService.pagination.totalPages
    );
    this.currentPage = this.helperService.markPageAsActive(paginationInfo);

    this.paginationRequest = new PaginationRequest(
      String(this.currentPage),
      String(this.dataStorageService.pagination.pageSize),
      this.searchTerm
    );
    this.loadActivities(this.displayFiltered);
  }

  loadActivities(showFiltered?: boolean): void {
    const user = this.authService.user.value;
    this.isLoading = true;

    this.httpService.getActivityEntities(user.id, this.paginationRequest)
      .subscribe((newActivities: ActivityEntity[]) => {

        const totalPages = this.dataStorageService.pagination.totalPages;
        if (totalPages === 0) {
          this.activityEntities.length = 0;
          this.isLoading = false;
          return;
        }

        const haveActivityEntitiesOnCurrentPage = newActivities.length;
        if (!haveActivityEntitiesOnCurrentPage) {
          this.initializeActivityEntities();
          return;
        }

        console.log(newActivities);
        this.updateDataStore(newActivities, Number(this.paginationRequest.page));

        if (showFiltered) {
          this.filteredActivityEntities = newActivities;
        }

        this.isLoading = false;
        this.errorMessage = null;
      },
        (errorRes: HttpErrorResponse) => {

          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;

          this.filteredActivityEntities.length = 0;
          this.activityEntities.length = 0;

          this.currentPage = 1;
          this.isLoading = false;
        });
  }

  updateDataStore(activities: ActivityEntity[], currentPage: number): void {
    const dataStore = this.dataStorageService;

    this.activityEntities = activities;
    dataStore.activityEntities = activities;

    dataStore.pagination.currentPage = currentPage;
    this.totalPages = dataStore.pagination.totalPages;
    this.totalCount = dataStore.pagination.totalCount;
  }

  createImagePath(imgPath: any): string {
    if (!imgPath) {
      return;
    }

    const fullImgPath = this.httpService.createImagePath(imgPath);
    return fullImgPath;
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

}
