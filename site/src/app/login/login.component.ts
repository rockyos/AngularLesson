import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { HttpService } from '../http.service';
import { Router } from '@angular/router';
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

  constructor(private service: HttpService, private router: Router, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      userEmail: ["", [Validators.required, Validators.email]],
      userPassword: ["", Validators.required],
      userCheckbox: [false]
    })
  }

  ngOnInit() {
    this.loggedOn();
  }

  loggedOn() {
    const token: string = localStorage.getItem('jwt');
    if (token) {
      this.router.navigate(['']);
    }
  }

  loginSend(email: string, pass: string, rememberMe: boolean) {
    this.service.loginPost(email, pass, rememberMe).subscribe(resualt => {
      this.jwt = resualt,
        localStorage.setItem('jwt', this.jwt),
        this.loggedOn()
    }, error => this.errorMessage = error['error']);
  }
}
