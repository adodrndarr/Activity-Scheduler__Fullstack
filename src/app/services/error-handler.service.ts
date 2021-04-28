import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  constructor() { }


  errorMessage = null;

  handleError(errorRes: HttpErrorResponse): void {
    this.errorMessage = 'An unknown error occured, there seems to be an issue with the connection, please try again later.';

    switch (errorRes.status) {
      case 400:
        this.errorMessage = 'Invalid info provided, could not complete the action.';
        break;
      case 401:
      case 403:
        this.errorMessage = 'You\'re not authorized to perform the action.';
        break;
      case 404:
        this.errorMessage = 'The ressource you were looking for, could not be found.';
        break;
      case 409:
          this.errorMessage = `The action can't be performed, since other entities depend on it.`;
          break;
      case 500:
        this.errorMessage = 'Internal Server Error, please try again later.';
        break;
    }
  }
}
