import { HttpClient, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Activity, ActivityEntity, BookedActivity, ScheduleActivity } from '../auth/Entities/Models/activity.model';
import { Pagination } from '../auth/Entities/Models/pagination.model';
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


  getActivityEntities(
    userId: string,
    page?: string,
    size?: string,
    searchTerm?: string
  ): Observable<ActivityEntity[]> {

    page = this.helperService.validateNumber(page, '1');
    size = this.helperService.validateNumber(size, '2');
    searchTerm = this.helperService.validateString(searchTerm, '');

    return this.http.get<ActivityEntity[]>(activitiesUrl, {
      params: {
        'userId': userId,
        'pageNumber': page,
        'pageSize': size,
        'includeName': searchTerm
      },
      observe: 'response'
    })
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
  

  getBookedActivities(
    activityEntityId: string,
    forDate: Date
  ): Observable<BookedActivity[]> {

    return this.http.get<BookedActivity[]>(`${scheduleUrl}/booked-activities`, {
      params: {
        'activityEntityId': activityEntityId,
        'forDate': forDate.toJSON()
      }
    })
      .pipe(
        tap(bookedActivities => {
          this.dataStorageService.bookedActivities = bookedActivities;
        })
      );
  }

  bookActivities(userId: string, activities: ScheduleActivity[]): Observable<any> {
    return this.http.post<Observable<any>>(`${scheduleUrl}/multiple`, activities, {
      params: {
        'userId': userId
      }
    });
  }

  getActivities(
    userId: string,
    page?: string,
    size?: string,
    searchTerm?: string
  ): Observable<Activity[]> {

    page = this.helperService.validateNumber(page, '1');
    size = this.helperService.validateNumber(size, '2');
    searchTerm = this.helperService.validateString(searchTerm, '');

    return this.http.get<Activity[]>(myActivitiesUrl, {
      params: {
        'userId': userId,
        'pageNumber': page,
        'pageSize': size,
        'includeName': searchTerm
      },
      observe: 'response'
    })
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


  getUsers(
    page?: string,
    size?: string,
    includeAdmin?: string,
    searchTerm?: string
  ): Observable<User[]> {

    page = this.helperService.validateNumber(page, '1');
    size = this.helperService.validateNumber(size, '2');
    searchTerm = this.helperService.validateString(searchTerm, '');
    includeAdmin = this.helperService.validateString(includeAdmin, 'false');

    return this.http.get<User[]>(usersUrl, {
      params: {
        'pageNumber': page,
        'pageSize': size,
        'includeAdmin': includeAdmin,
        'includeName': searchTerm
      },
      observe: 'response'
    })
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
