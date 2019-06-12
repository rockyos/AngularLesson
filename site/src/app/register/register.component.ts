import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  errorMessage: string;
  registerForm: FormGroup;
  jwt: any;

  constructor(private service: HttpService, private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      registerEmail: ["", Validators.required, Validators.email],
      registerPass: ["", Validators.required],
      registerPassConfirm: ["", Validators.required]
    });
  }

  ngOnInit() {
  }

  registerSend(email: string, pass: string, passconfirm: string) {
    this.service.registerPost(email, pass, passconfirm).subscribe(resualt => {this.jwt = resualt, console.log(this.jwt)},
     error => this.errorMessage =  error['error']['value']['message']);
  }
}
