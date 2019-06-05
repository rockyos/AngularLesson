import { AbstractControl, ValidatorFn } from '@angular/forms';


export function PasswordValidation(control: AbstractControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        let password = control.get('registerPass').value;
        let confirmPassword = control.get('registerPassConfirm').value;
        console.log("password: " + password + " confirmPassword: "+ confirmPassword)
        if (password != confirmPassword) {
            control.get('registerPassConfirm').setErrors({ PasswordValidation: true })
        } else {
            return null;
        }
    }
}
