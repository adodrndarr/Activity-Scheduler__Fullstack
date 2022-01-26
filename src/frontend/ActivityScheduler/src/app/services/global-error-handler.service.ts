import { ErrorHandler, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor() { }


  handleError(error: Error) {
      const err = {
          message: (error.message) ? error.message : error.toString(),
          stack: (error.stack) ? error.stack : ''
      };
      console.error(err);

      // Notify the user
      alert('Oops something went wrong, an unknown error ocurred, please try again later.');
      location.reload();
  }
}
