import { HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Swal, { SweetAlertResult } from 'sweetalert2';
import { Pagination } from '../auth/Entities/Models/pagination.model';
import { DataStorageService } from './data-storage.service';


@Injectable({
  providedIn: 'root'
})
export class HelperService {
  constructor(
    private router: Router,
    private dataStorageService: DataStorageService
  ) { }


  navigateTo(path: string): void {
    this.router.navigateByUrl(`/${path}`);
    window.scrollTo(0, 0);
  }

  filterByName(text: string, searchTerm: string): boolean {
    const textToCheck = text.toLowerCase();
    const term = searchTerm.toLowerCase();

    return textToCheck.includes(term);
  }

  createAlert(title: string, imageUrl?: string)
    : Promise<SweetAlertResult<any>> {
    if (imageUrl) {
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
    else {
      return Swal.fire({
        title: `${title}`,
        text: 'Please confirm to continue...',
        showCancelButton: true,
        confirmButtonText: `Yes, delete it.`,
        confirmButtonColor: '#e62e00',
        cancelButtonText: `No, keep it.`,
        cancelButtonColor: '#999999'
      });
    }
  }

  createCancelAlert(title: string): Promise<SweetAlertResult<any>> {
    return Swal.fire({
      title: `${title}`,
      text: 'Please confirm to continue...',
      showCancelButton: true,
      confirmButtonText: `Yes, cancel it.`,
      confirmButtonColor: '#e62e00',
      cancelButtonText: `No, leave it.`,
      cancelButtonColor: '#999999'
    });
  }

  getPaginationElements(): any {
    return document.querySelectorAll('.page-item');
  }

  markPageAsActive(
    value: string,
    selectedPage: number,
    currentPage: number,
    totalPages: number
  ): number {

    const elems: any = this.getPaginationElements();

    currentPage = this.validatePage(currentPage, totalPages, value, selectedPage);
    this.markElements(elems, currentPage);

    return currentPage;
  }

  validatePage(page: number, limit: number, value: string, selectedPage: number) {
    if (value === 'prev') {
      page = (page <= 1) ? 1 : page -= 1;
      page = (page <= limit) ? page : 1;

      return page;
    }

    if (value === 'next') {
      page = (page < 1) ? 1 : page += 1;
      page = (page <= limit) ? page : 1;
      return page;
    }

    return selectedPage;
  }

  markElements(elems: any, index: number): void {
    elems.forEach(el => el.classList.remove('active'));
    if (elems[index]) {
      elems[index].className = 'page-item active';
    }
  }

  toPagination(obj: any): Pagination {
    const pagination = new Pagination();
    pagination.currentPage = (obj.CurrentPage === 0) ? 1 : obj.CurrentPage;
    pagination.pageSize = obj.PageSize;
    pagination.totalCount = obj.TotalCount;
    pagination.totalPages = obj.TotalPages;
    pagination.hasPrevious = obj.HasPrevious;
    pagination.hasNext = obj.HasNext;

    return pagination;
  }

  validateNumber(value: string, failSafeValue: string): string {
    const finalValue = isNaN(+value) ? failSafeValue : value;
    return finalValue;
  }

  validateString(value: string, failSafeValue: string): string {
    const finalValue = !value ? failSafeValue : value;
    return finalValue;
  }

  checkCurrentPageForChange(pagination: Pagination): void {
    const currentPageIsInvalid = pagination.currentPage > pagination.totalPages &&
                                 pagination.totalPages !== 0;

    if (currentPageIsInvalid) {
      this.dataStorageService.pagination.currentPage = pagination.totalPages;
    }
  }

  handlePagination(obj: any): void {
    if (obj) {
      const pagination = this.toPagination(obj);
      this.dataStorageService.pagination = pagination;
    }
    else {
      this.dataStorageService.pagination.totalPages -= 1;
    }

    this.checkCurrentPageForChange(this.dataStorageService.pagination);
  }
}
