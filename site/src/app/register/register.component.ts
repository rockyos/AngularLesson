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
    this.service.registerPost(email, pass, passconfirm).subscribe(resualt => {
      this.errorMessage =  resualt['value']['message']
    });
  }
}
