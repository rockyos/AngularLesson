import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { PasswordValidation } from '../pass-validation';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  errorMessage: string;
  registerForm: FormGroup;

  constructor(private service: HttpService, private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      registerEmail: ["", [Validators.required, Validators.email]],
      registerPass: ["", Validators.required],
      registerPassConfirm: ["", Validators.required]
    }, { validator: PasswordValidation });
  }

  ngOnInit() {
  }

  registerSend(email: string, password: string, confirmpassword: string) {
    this.service.registerPost(email, password, confirmpassword).subscribe(resualt => {
      this.errorMessage =  resualt['value']['message']
    });
  }
}
