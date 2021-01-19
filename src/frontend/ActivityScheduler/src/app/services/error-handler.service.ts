import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  constructor() { }


  errorMessage = null;

  handleError(errorRes: HttpErrorResponse): void {
    this.errorMessage = 'An unknown error occured, please try again later.';

    switch (errorRes.status) {
      case 400:
        this.errorMessage = 'Invalid info provided, could not complete the action.';
        break;
      case 401:
      case 403:
        this.errorMessage = 'You\'re not authorized to perform the action.';
        break;
      case 500:
        this.errorMessage = 'Internal Server Error, please try again later.';
        break;
    }
  }
}
