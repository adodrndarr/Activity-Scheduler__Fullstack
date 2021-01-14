import { AbstractControl } from '@angular/forms';


export function matchPassword(control: AbstractControl): void {
    const passwordControl = control.get('password');
    const confirmPwdControl = control.get('confirmPassword');

    let password;
    if (passwordControl) {
        password = passwordControl.value;
    }

    let confirmPassword;
    if (confirmPwdControl) {
        confirmPassword = confirmPwdControl.value;
    }

    if (confirmPwdControl.errors && !confirmPwdControl.errors.isInvalidMatch) {
        return;
    }

    if (password !== confirmPassword) {
        confirmPwdControl.setErrors({ isInvalidMatch: true });
    }
    else {
        confirmPwdControl.setErrors(null);
    }
}
