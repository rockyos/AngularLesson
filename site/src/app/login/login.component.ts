import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
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
  url = `${environment.apiUrl}`;
  clientUrl = `${environment.clientUrl}`;
  loginForm: FormGroup;
  jwt: string;

  constructor(private service: HttpService, private activateRoute: ActivatedRoute, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      userEmail: ["", [Validators.required, Validators.email]],
      userPassword: ["", Validators.required],
      userCheckbox: [false]
    })
  }

  ngOnInit() {
  }

  loginSend(email: string, pass: string, rememberMe: boolean) {
    this.service.loginPost(email, pass, rememberMe).subscribe(resualt => {
      this.jwt = resualt, localStorage.setItem('jwt', this.jwt)},
      error => this.errorMessage = error['error']);
  }
}
