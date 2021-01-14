import { Component } from '@angular/core';


@Component({
  selector: 'app-loading-spinner',
  template: `
    <div class="text-center">
        <div class="spinner-border" role="status" style="width: 50px; height: 50px;">
            <span class="sr-only btn-info"></span>
        </div><br /><br />
        <div>
            <h4>Processing...</h4>
        </div>
    </div>
  `
})
export class LoadingSpinnerComponent {
}
