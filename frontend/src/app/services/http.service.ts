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
  constructor(private htpp: HttpClient) { }


  getActivityEntities(): Observable<ActivityEntity[]> {
    return this.htpp.get<ActivityEntity[]>(activitiesUrl);
  }
}
