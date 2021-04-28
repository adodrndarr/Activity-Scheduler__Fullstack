import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/auth/auth.service';
import { ActivityEntity, ScheduleActivity } from 'src/app/auth/Entities/Models/activity.model';
import { DefaultDate } from 'src/app/auth/Entities/Models/date.model';
import { DataStorageService } from 'src/app/services/data-storage.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { HelperService } from 'src/app/services/helper.service';
import { HttpService } from 'src/app/services/http.service';
import Swal from 'sweetalert2';
import { ScheduleActivityService } from './schedule-activity.service';


@Component({
  selector: 'app-schedule-activity',
  templateUrl: './schedule-activity.component.html'
})
export class ScheduleActivityComponent implements OnInit {
  constructor(
    private dataStorageService: DataStorageService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private errorHandlerService: ErrorHandlerService,
    private authService: AuthService,
    private helperService: HelperService,
    private scheduleService: ScheduleActivityService
  ) { }


  activities: ActivityEntity[];
  dropdownForm: FormGroup;

  defaultTimes: DefaultDate[] = this.scheduleService.createDefaultDates();
  activitiesToBook: ScheduleActivity[] = [];

  scheduleMode = false;
  isLoading = false;
  errorMessage = null;
  canBook = false;

  ngOnInit(): void {
    this.initializeActivityEntities();
    this.initializeDropdownForm();
    this.scheduleService.onScrollDownShowArrow();
  }

  onSubmit(): void {
    this.handleSubmittedSearchActivityForm();
  }


  initializeActivityEntities(): void {
    this.isLoading = true;
    this.httpService.getAllActivityEntities()
        .subscribe(newActivities => {
          this.activities = newActivities;
          this.dataStorageService.activityEntities = newActivities;

          this.isLoading = false;
          this.errorMessage = null;
        },
          (errorRes: HttpErrorResponse) => {
            console.log(errorRes);
            this.errorHandlerService.handleError(errorRes);

            this.errorMessage = this.errorHandlerService.errorMessage;
            this.isLoading = false;
          });
  }

  initializeDropdownForm(): void {
    this.dropdownForm = this.formBuilder.group({
      'activityEntity': [null, Validators.required],
      'date': [null, Validators.required]
    });

    this.dropdownForm.controls['activityEntity'].patchValue(null);
  }

  private handleSubmittedSearchActivityForm(): void {
    if (!this.dropdownForm.valid) {
      return;
    }

    this.getBookedActivities();
  }

  getBookedActivities(): void {
    this.isLoading = true;
    this.resetScheduledActivities();

    const activityEntityId = this.dropdownForm.controls['activityEntity'].value;
    const date = new Date(this.dropdownForm.controls['date'].value);

    this.httpService.getBookedActivities(activityEntityId, date)
      .subscribe((bookedActivitiesRes: any) => {
        console.log(bookedActivitiesRes);
        this.scheduleMode = true;

        if (bookedActivitiesRes.length > 0) {
          this.scheduleService.modifyTimeAvailability(this.defaultTimes);
        }
        else if (bookedActivitiesRes.isValid) {
          this.defaultTimes.forEach(dateTime => dateTime.isFreeToSchedule = true);
        }
        else {
          this.errorMessage = 'You can\'t book activity in the past, unless you\'re using a time machine.';

          this.scheduleMode = false;
          this.isLoading = false;

          return;
        }

        this.isLoading = false;
        this.errorMessage = null;
      },
        (errorRes: HttpErrorResponse) => {
          console.log(errorRes);
          this.errorHandlerService.handleError(errorRes);

          this.errorMessage = this.errorHandlerService.errorMessage;
          this.isLoading = false;
          this.scheduleMode = false;
        });
  }

  onSchedule(defaultDate: DefaultDate, index: number): void {
    const date: Date = new Date(this.dropdownForm.controls['date'].value);

    const activityId = this.dropdownForm.controls['activityEntity'].value;
    const userId = this.authService.user.value.id;

    const scheduleActivity = this.scheduleService
      .createActivity(defaultDate, date, activityId, userId, index);

    this.activitiesToBook = [...this.activitiesToBook, scheduleActivity];
    this.defaultTimes[index].isScheduled = true;

    this.canBook = this.activitiesToBook.length > 0;
  }

  onDetails(): void {
    const timeDetails = this.scheduleService.createTimeDetails(this.activitiesToBook);
    this.showTimeInfo(timeDetails);
  }

  showTimeInfo(timeDetails: string): void {
    Swal.fire({
      title: `<i>Scheduled Time:</i>`,
      html: `<pre><b>${timeDetails}</b></pre>`,
      customClass: {
        popup: 'details'
      },
      showCancelButton: false,
      confirmButtonText: `Okay`,
      confirmButtonColor: '#1aa3ff'
    });
  }

  onBook(): void {
    const user = this.authService.user.value;

    this.isLoading = true;
    this.defaultTimes.forEach(date => date.isScheduled = false);

    this.httpService.bookActivities(user.id, this.activitiesToBook)
      .subscribe(res => {
        console.log(res);

        if (res.unsuccessfulInfo.length > 0) {
          this.errorMessage = res.unsuccessfulInfo[0];
        }

        this.resetScheduledActivities();
        this.isLoading = false;
        this.canBook = false;

        if (res.unsuccessfulInfo.length === 0) {
          this.helperService.navigateTo('my-activities');
        }
      });
  }

  resetScheduledActivities(): void {
    this.activitiesToBook.length = 0;
    this.defaultTimes.forEach(date => date.isScheduled = false);
  }

  onCancel(index: number): void {
    this.defaultTimes[index].isScheduled = false;
    this.activitiesToBook = this.activitiesToBook.filter(a => a.currentId !== index);

    this.canBook = this.activitiesToBook.length > 0;
  }

  onToTop(): void {
    this.scheduleService.toTopOfPage();
  }

}
