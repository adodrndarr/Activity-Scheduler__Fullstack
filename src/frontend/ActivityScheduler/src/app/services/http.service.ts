import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Activity, ActivityEntity, BookedActivity, ScheduleActivity } from '../auth/Entities/Models/activity.model';
import { Pagination } from '../auth/Entities/Models/pagination.model';
import { PaginationRequest } from '../auth/Entities/Models/requests/pagination-request.model';
import { User } from '../auth/Entities/Models/user.model';
import { DataStorageService } from './data-storage.service';
import { HelperService } from './helper.service';


const activitiesUrl = `${environment.baseUrl}/activityEntities`;
const usersUrl = `${environment.baseUrl}/manageUsers`;
const scheduleUrl = `${environment.baseUrl}/scheduleActivity`;
const myActivitiesUrl = `${environment.baseUrl}/myActivities`;

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private dataStorageService: DataStorageService,
    private helperService: HelperService
  ) { }


  getActivityEntities(userId: string, paginationRequest: PaginationRequest): Observable<ActivityEntity[]> {

    let { page, size, searchTerm } = paginationRequest;

    page = this.helperService.validateNumber(page, '1');
    size = this.helperService.validateNumber(size, '2');
    searchTerm = this.helperService.validateString(searchTerm, '');

    const params = {
      'userId': userId,
      'pageNumber': page,
      'pageSize': size,
      'includeName': searchTerm
    };

    return this.http
      .get<ActivityEntity[]>(activitiesUrl, { params: params, observe: 'response' })
      .pipe(
        tap(res => {
          const obj: Pagination = JSON.parse(res.headers.get('x-pagination'));
          this.helperService.handlePagination(obj);
        }),
        map(res => res.body)
      );
  }

  getAllActivityEntities(): Observable<ActivityEntity[]> {
    return this.http.get<ActivityEntity[]>(`${activitiesUrl}/all`);
  }

  getActivityEntityById(id: string): Observable<ActivityEntity> {
    return this.http.get<ActivityEntity>(`${activitiesUrl}/single`, {
      params: {
        'id': id
      }
    });
  }

  editActivityEntity(id: string, activityEntityToUpdate: ActivityEntity): Observable<any> {
    return this.http.put<any>(activitiesUrl, activityEntityToUpdate, {
      params: { 'activityId': id }
    });
  }

  createActivityEntity(newActivityEntity: ActivityEntity): Observable<any> {
    return this.http.post<any>(activitiesUrl, newActivityEntity);
  }

  deleteActivityEntity(id: string): Observable<any> {
    return this.http.delete<any>(activitiesUrl, {
      params: { 'activityEntityId': id }
    });
  }


  uploadFile(formData: FormData): Observable<any> {
    return this.http.post<any>(`${activitiesUrl}/upload`, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

  createImagePath(imgPath: any): string {
    return `${environment.baseUrl}/${imgPath}`;
  }


  getBookedActivities(activityEntityId: string, forDate: Date): Observable<BookedActivity[]> {
    const params = {
      'activityEntityId': activityEntityId,
      'forDate': forDate.toJSON()
    };

    return this.http.get<BookedActivity[]>(`${scheduleUrl}/booked-activities`, { params: params })
      .pipe(
        tap(bookedActivities => this.dataStorageService.bookedActivities = bookedActivities)
      );
  }

  bookActivities(userId: string, activities: ScheduleActivity[]): Observable<any> {
    return this.http.post<Observable<any>>(`${scheduleUrl}/multiple`, activities, {
      params: {
        'userId': userId
      }
    });
  }

  getActivities(userId: string, paginationRequest: PaginationRequest): Observable<Activity[]> {

    let { page, size, searchTerm } = paginationRequest;

    page = this.helperService.validateNumber(page, '1');
    size = this.helperService.validateNumber(size, '2');
    searchTerm = this.helperService.validateString(searchTerm, '');

    const params = {
      'userId': userId,
      'pageNumber': page,
      'pageSize': size,
      'includeName': searchTerm
    };

    return this.http
      .get<Activity[]>(myActivitiesUrl, { params: params, observe: 'response' })
      .pipe(
        tap(res => {
          const obj: Pagination = JSON.parse(res.headers.get('x-pagination'));
          this.helperService.handlePagination(obj);
        }),
        map(res => res.body)
      );
  }



  cancelActivity(activityId: string): Observable<any> {
    return this.http.delete<Observable<any>>(myActivitiesUrl, {
      params: {
        'activityId': activityId
      }
    });
  }


  getUsers(includeAdmin?: boolean, paginationRequest?: PaginationRequest): Observable<User[]> {

    if (!paginationRequest)
      paginationRequest = new PaginationRequest('1', '2', '');
    else {
      paginationRequest.page = this.helperService.validateNumber(paginationRequest.page, '1');
      paginationRequest.size = this.helperService.validateNumber(paginationRequest.size, '2');
      paginationRequest.searchTerm = this.helperService.validateString(paginationRequest.searchTerm, '');
    }

    const params = {
      'pageNumber': paginationRequest.page,
      'pageSize': paginationRequest.size,
      'includeAdmin': this.helperService.validateString(String(includeAdmin), 'false'),
      'includeName': paginationRequest.searchTerm
    };

    return this.http
      .get<User[]>(usersUrl, { params: params, observe: 'response' })
      .pipe(
        tap(res => {
          const obj: Pagination = JSON.parse(res.headers.get('x-pagination'));
          this.helperService.handlePagination(obj);
        }),
        map(res => res.body)
      );
  }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${usersUrl}/single`, {
      params: {
        'id': id
      }
    });
  }

  editUser(id: string, userToUpdate: User): Observable<any> {
    return this.http.put<any>(usersUrl, userToUpdate, {
      params: { 'userId': id }
    });
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete<any>(usersUrl, {
      params: { 'userId': id }
    });
  }

}
