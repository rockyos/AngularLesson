import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpService } from '../http.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errorMessage: string;
  returnUrl: string;
  url = `${environment.apiUrl}`;
  loginForm: FormGroup;

  constructor(private service: HttpService, private activateRoute: ActivatedRoute, private fb: FormBuilder) {
    this.returnUrl = this.activateRoute.snapshot.queryParamMap.get("ReturnUrl");
    this.loginForm = this.fb.group({
      userEmail: ["", [Validators.required, Validators.email]],
      userPassword: ["", Validators.required],
      userCheckbox: [false]
    })
  }

  ngOnInit() {
  }

  loginSend(email: string, pass: string, rememberMe: boolean) {
    this.service.loginPost(email, pass, rememberMe, this.returnUrl).subscribe(resualt => {
      this.errorMessage =  resualt['value']['message']
    });
  }
}
