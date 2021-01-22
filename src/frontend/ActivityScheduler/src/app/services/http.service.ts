import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ActivityEntity } from '../auth/Entities/Models/activityEntity.model';


const activitiesUrl = `${environment.baseUrl}/activityEntities`;

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private htpp: HttpClient
  ) { }


  getActivityEntities(): Observable<ActivityEntity[]> {
    return this.htpp.get<ActivityEntity[]>(activitiesUrl);
  }

  editActivityEntity(id: string, activityEntityToUpdate: ActivityEntity): Observable<any> {
    return this.htpp.put<any>(activitiesUrl, activityEntityToUpdate, {
      params: { 'activityId': id }
    });
  }

  createActivityEntity(newActivityEntity: ActivityEntity): Observable<any> {
    return this.htpp.post<any>(activitiesUrl, newActivityEntity);
  }

  deleteActivityEntity(id: string): Observable<any> {
    return this.htpp.delete<any>(activitiesUrl, {
      params: { 'activityEntityId': id }
    });
  }
}
