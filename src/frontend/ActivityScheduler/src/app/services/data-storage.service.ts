import { Injectable } from '@angular/core';
import { Activity, ActivityEntity, BookedActivity } from '../auth/Entities/Models/activity.model';
import { Pagination } from '../auth/Entities/Models/pagination.model';
import { User } from '../auth/Entities/Models/user.model';


@Injectable({
  providedIn: 'root'
})
export class DataStorageService {
  constructor() { }


  pagination: Pagination = new Pagination(1, 1, 2);
  users: User[] = [];

  activities: Activity[] = [];
  activityEntities: ActivityEntity[] = [];
  bookedActivities: BookedActivity[] = [];

  searchTerm: string;
  currentImagePath: string;
  checked = false;
}
