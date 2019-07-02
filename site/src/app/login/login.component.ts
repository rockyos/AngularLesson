import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { HttpService } from '../http.service';
import { environment } from 'src/environments/environment.prod';
import { TokenService } from '../token.service';
import { Router } from '@angular/router';


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
  rememberMe: boolean = false;

  constructor(private service: HttpService, private token: TokenService, private router: Router, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      userEmail: ["", [Validators.required, Validators.email]],
      userPassword: ["", Validators.required],
      userCheckbox: [false]
    });
  }

  ngOnInit() {
    this.token.loggedOn();
  }

  loginSend(email: string, pass: string) {
    this.rememberMe = this.loginForm.controls['userCheckbox'].value;
    this.token.sessionOrLocalStorage(this.rememberMe);
    this.service.loginPost(email, pass).subscribe(resualt => {
      this.jwt = resualt,
        this.token.setToken(this.jwt),
        this.token.loggedOn();
    }, error => { typeof error['error'] === 'string' ? this.errorMessage = error['error'] : this.errorMessage = "error" });
  }

  googleBtn() {
    this.service.googleGet().subscribe(
      resualt => {
        this.jwt = resualt,
          this.token.setToken(this.jwt),
          this.token.loggedOn()
      }, error => {
        error['error'] === "401 Unauthorized" ? this.router.navigate(['Account/RegisterExternal']) : null,
          typeof error['error'] === 'string' ? this.errorMessage = error['error'] : this.errorMessage = "error"
      }
    );
  }

  facebookBtn() {
    this.service.facebookGet().subscribe(
      resualt => {
        this.jwt = resualt,
          this.token.setToken(this.jwt),
          this.token.loggedOn()
      }, error => {
        error['error'] === "401 Unauthorized" ? this.router.navigate(['Account/RegisterExternal']) : null,
          typeof error['error'] === 'string' ? this.errorMessage = error['error'] : this.errorMessage = "error"
      }
    );
  }
}
