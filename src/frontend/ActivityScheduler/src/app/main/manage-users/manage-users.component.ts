import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { PaginationInfo } from 'src/app/auth/Entities/Models/pagination.model';
import { PaginationRequest } from 'src/app/auth/Entities/Models/requests/pagination-request.model';
import { User } from 'src/app/auth/Entities/Models/user.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';


@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html'
})
export class ManageUsersComponent implements OnInit, OnDestroy, AfterViewChecked {
  constructor(
    private dataStorageService: DataStorageService,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private helperService: HelperService,
    private authService: AuthService
  ) { }


  userSub: Subscription;

  users: User[] = [];
  filteredUsers: User[] = [];

  errorMessage = null;
  isLoading = true;
  isAdmin = false;

  searchTerm: string;
  paginationRequest: PaginationRequest = {};
  includeAdmin = false;
  displayFiltered = false;

  pageItems = [1, 2, 3, 4, 5, 6, 7, 8];
  currentPage = 1;
  totalPages: number;
  totalCount: number;

  ngOnInit() {
    this.initializeUsers();
    this.checkUserRole();
  }

  ngAfterViewChecked(): void {
    const elems = this.helperService.getPaginationElements();
    this.helperService.markElements(
      elems,
      this.dataStorageService.pagination.currentPage
    );
  }

  initializeUsers() {
    this.updateState();
    this.updateUsers(this.dataStorageService.pagination.currentPage);
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
      this.updateUsers(this.currentPage);

      return;
    }

    this.displayFiltered = true;
    this.currentPage = 1;
    this.updateUsers(this.currentPage);
  }

  onClear(): void {
    this.searchTerm = '';
    this.dataStorageService.searchTerm = this.searchTerm;
    this.displayFiltered = false;

    this.updateUsers(this.currentPage);
  }

  onDelete(user: User): void {
    if (!this.isAdmin)
      return;

    this.isLoading = true;
    const title = `Delete ${user.userName}?`;

    this.helperService.createAlert(title)
      .then((choice) => {

        if (choice.isConfirmed)
          this.deleteUser(user);
        else
          this.isLoading = false;
      });
  }

  private deleteUser(user: User): void {
    this.httpService.deleteUser(user.id)
      .subscribe(res => {

        console.log(res);

        this.initializeUsers();
        this.isLoading = false;
      },
        (errorRes: HttpErrorResponse) => {

          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);

          this.errorMessage = this.errorHandlerService.errorMessage;
          this.isLoading = false;
        });
  }

  updateUsers(selectedPage: number, value?: string): void {
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
    this.loadUsers(this.displayFiltered, this.includeAdmin);
  }

  onCheckboxChange(): void {
    this.includeAdmin = !this.includeAdmin;
    this.dataStorageService.checked = this.includeAdmin;

    if (!this.includeAdmin) {
      this.displayFiltered = false;
      this.updateUsers(this.currentPage);

      return;
    }

    this.displayFiltered = true;
    this.currentPage = 1;
    this.updateUsers(this.currentPage);
  }

  loadUsers(showFiltered?: boolean, includeAdmin = false): void {

    this.isLoading = true;
    this.httpService.getUsers(includeAdmin, this.paginationRequest)
      .subscribe((newUsers: User[]) => {

        const totalPages = this.dataStorageService.pagination.totalPages;
        if (totalPages === 0) {
          this.users.length = 0;
          this.isLoading = false;
          return;
        }

        const haveUsersOnCurrentPage = newUsers.length;
        if (!haveUsersOnCurrentPage) {
          this.initializeUsers();
          return;
        }

        console.log(newUsers);
        this.updateDataStore(newUsers, Number(this.paginationRequest.page));

        if (showFiltered) {
          this.filteredUsers = newUsers;
        }

        this.isLoading = false;
        this.errorMessage = null;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);

          this.errorHandlerService.handleError(errorRes);
          this.errorMessage = this.errorHandlerService.errorMessage;

          this.filteredUsers.length = 0;
          this.users.length = 0;

          this.currentPage = 1;
          this.isLoading = false;
        });
  }

  updateDataStore(users: User[], currentPage: number): void {
    const dataStore = this.dataStorageService;

    this.users = users;
    dataStore.users = users;

    dataStore.pagination.currentPage = currentPage;
    this.totalPages = dataStore.pagination.totalPages;
    this.totalCount = dataStore.pagination.totalCount;
  }

  updateState(): void {
    this.searchTerm = this.dataStorageService.searchTerm;
    this.includeAdmin = this.dataStorageService.checked;

    const checkboxes: any = document.querySelectorAll('.checkbox input');
    checkboxes.forEach(checkbox => checkbox.checked = this.includeAdmin);
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

}
