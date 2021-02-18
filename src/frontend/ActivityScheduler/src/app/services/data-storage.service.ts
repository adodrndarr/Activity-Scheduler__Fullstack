import { Injectable } from '@angular/core';
import { Activity, ActivityEntity, BookedActivity } from '../auth/Entities/Models/activity.model';
import { Pagination } from '../auth/Entities/Models/pagination.model';
import { User } from '../auth/Entities/Models/user.model';


@Injectable({
  providedIn: 'root'
})
export class DataStorageService {
  constructor() { }


  activityEntities: ActivityEntity[] = [];
  users: User[] = [];
  activities: Activity[] = [];

  pagination: Pagination = new Pagination(1, 1, 2);
  bookedActivities: BookedActivity[] = [];

  searchTerm: string;
  checked = false;
}
