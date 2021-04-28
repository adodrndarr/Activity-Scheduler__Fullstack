import { Injectable } from '@angular/core';
import { ScheduleActivity } from 'src/app/auth/Entities/Models/activity.model';
import { DefaultDate } from 'src/app/auth/Entities/Models/date.model';
import { DataStorageService } from 'src/app/services/data-storage.service';


@Injectable({
  providedIn: 'root'
})
export class ScheduleActivityService {
  constructor(
    private dataStorageService: DataStorageService
  ) { }


  createDefaultDates(): DefaultDate[] {
    const defaultDates: DefaultDate[] = [];

    for (let i = 0; i < 11; i++) {
      const date = new Date();
      date.setHours(7 + i, 0, 0, 0);

      const defaultDate = new DefaultDate(date, false, false);
      defaultDates.push(defaultDate);
    }

    return defaultDates;
  }

  modifyTimeAvailability(defaultTimes: DefaultDate[]): void {
    const bookedActivities = this.dataStorageService.bookedActivities;

    for (const defaultDate of defaultTimes) {
      for (const bookedActivity of bookedActivities) {
        const bookedStartDate = new Date(bookedActivity.startTime);
        const bookedEndDate = new Date(bookedActivity.endTime);

        const defaultDateHours = defaultDate.date.getHours();

        const bookedStartDateHours = bookedStartDate.getHours();
        const bookedEndDateHours = bookedEndDate.getHours();
        const bookedEndDateMinutes = bookedEndDate.getMinutes();

        const startsBeforeBookedTime = defaultDateHours < bookedStartDateHours;
        const startsAfterBookedTime = defaultDateHours > bookedEndDateHours;
        const startsAfterBookedTimeAndMinutes = defaultDateHours >= bookedEndDateHours &&
          bookedEndDateMinutes === 0;

        if (startsBeforeBookedTime ||
          startsAfterBookedTime ||
          startsAfterBookedTimeAndMinutes
        ) {
          defaultDate.isFreeToSchedule = true;
        }
        else {
          defaultDate.isFreeToSchedule = false;
          break;
        }
      }
    }

  }

  createTimeDetails(activities: ScheduleActivity[]): string {
    let timeDetails = ``;
    for (const bookedActivity of activities) {
      const startTime = bookedActivity.startTime.getHours();
      const startTimeMinutes = bookedActivity.startTime.getMinutes();

      const endTime = bookedActivity.endTime.getHours();
      const endTimeMinutes = bookedActivity.endTime.getMinutes();

      const fullInfo = `${startTime}:${startTimeMinutes}0 - ${endTime}:${endTimeMinutes}0 \n`;
      timeDetails += fullInfo;
    }

    return timeDetails;
  }

  createActivity(
    defaultDate: DefaultDate,
    date: Date,
    activityId: string,
    userId: string,
    index: number
  ): ScheduleActivity {
    const bookedDateHour = defaultDate.date.getHours();
    date.setHours(bookedDateHour);

    const bookedForDate = date;
    const startTime = date;

    const endTime = new Date(date);
    endTime.setHours(date.getHours() + 1);

    const scheduleActivity = new ScheduleActivity(
      bookedForDate,
      startTime,
      endTime,
      activityId,
      userId,
      index
    );

    return scheduleActivity;
  }

  onScrollDownShowArrow(): void {
    var arrowUp = document.getElementById("arrow-up");

    window.onscroll = (ev => {
      const userScrolledDown = document.body.scrollTop > 450 ||
                               document.documentElement.scrollTop > 450;

      if (userScrolledDown) {
        arrowUp.style.display = "block";
      } else {
        arrowUp.style.display = "none";
      }
    });
  }

  toTopOfPage(): void {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }
}
