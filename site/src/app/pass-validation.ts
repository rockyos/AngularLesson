import { AbstractControl, ValidatorFn } from '@angular/forms';


export function PasswordValidation(control: AbstractControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        console.log("work in pass");
        let password = control.get('registerPass').value;
        let confirmPassword = control.get('registerPassConfirm').value;
        if (password != confirmPassword) {
            control.get('registerPassConfirm').setErrors({ PasswordValidation: true })
        } else {
            return null;
        }
    }
}
