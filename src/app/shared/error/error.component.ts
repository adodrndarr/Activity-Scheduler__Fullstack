import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-error',
  templateUrl: './error.component.html'
})
export class ErrorComponent {
  @Input() error = 'An unknown error occured, please try again later.';
}
