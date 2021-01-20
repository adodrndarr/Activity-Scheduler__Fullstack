import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Swal, { SweetAlertResult } from 'sweetalert2';


@Injectable({
  providedIn: 'root'
})
export class HelperService {
  constructor(private router: Router) { }


  navigateTo(path: string): Promise<boolean> {
    this.router.navigateByUrl(`/${path}`);
    window.scrollTo(0, 0);

    return Promise.resolve(true);
  }

  navigateAndUpdateTo(path: string): void {
    this.navigateTo(path)
      .then(_ => location.reload());
  }

  createAlert(title: string, imageUrl: string)
  : Promise<SweetAlertResult<any>> {
    return Swal.fire({
      title: `${title}`,
      imageUrl: `${imageUrl}`,
      imageWidth: 400,
      imageHeight: 200,
      text: 'Please confirm to continue...',
      showCancelButton: true,
      confirmButtonText: `Yes, delete it.`,
      confirmButtonColor: '#e62e00',
      cancelButtonText: `No, keep it.`,
      cancelButtonColor: '#999999'
    });
  }
}
