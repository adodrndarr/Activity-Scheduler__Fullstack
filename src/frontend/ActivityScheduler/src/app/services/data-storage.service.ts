import { Injectable } from '@angular/core';
import { ActivityEntity } from '../auth/Entities/Models/activityEntity.model';


@Injectable({
  providedIn: 'root'
})
export class DataStorageService {
  constructor() { }


  activityEntities: ActivityEntity[] = [];
  selectedActivity: ActivityEntity = null;
}
