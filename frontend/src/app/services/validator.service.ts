import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';


@Injectable({
  providedIn: 'root'
})
export class ValidatorService {
  constructor() { }


  public isInvalidInput(fieldName: string, form: FormGroup): boolean {
    const invalid = form.controls[fieldName].invalid;
    const isDirty = form.controls[fieldName].dirty;
    const isTouched = form.controls[fieldName].touched;

    return invalid && (isDirty || isTouched);
  }

  public isRequired(fieldName: string, form: FormGroup): boolean {
    const errors = form.controls[fieldName].errors;

    return errors.required;
  }

  public isInvalidFormat(fieldName: string, form: FormGroup): boolean {
    const errors = form.controls[fieldName].errors;

    return errors.pattern;
  }

  public isInvalidMatch(fieldName: string, form: FormGroup): boolean {
    const control = form.controls[fieldName];
    let isInvalidMatch = false;

    if (control.errors) {
      isInvalidMatch = control.errors.isInvalidMatch;
    }

    return isInvalidMatch && control.dirty;
  }
}
