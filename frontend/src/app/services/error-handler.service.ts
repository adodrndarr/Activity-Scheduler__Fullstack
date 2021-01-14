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
      case 401:
        this.errorMessage = 'You\'re not authorized to access the ressource.';
        break;
      case 500:
        this.errorMessage = 'Internal Server Error, please try again later.';
        break;
    }
  }
}
