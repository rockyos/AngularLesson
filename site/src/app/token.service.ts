import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private router: Router) { }

  tokenKey: string = 'jwt';
  localStorage: boolean = false;

  sessionOrLocalStorage(rememberMe: boolean) {
    this.localStorage = rememberMe;
  }

  loggedOn() {
    var token = this.getToken();
    if (token) {
      this.router.navigate(['']);
    }
  }

  setToken(jwt: string) {
    if (this.localStorage) {
      localStorage.setItem(this.tokenKey, jwt);
    } else {
      sessionStorage.setItem(this.tokenKey, jwt);
    }
  }

  getToken(): string {
    var token;
    token = localStorage.getItem(this.tokenKey);
    if (!token) {
      token = sessionStorage.getItem(this.tokenKey)
    }
    return token;
  }

  logOut() {
    sessionStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['Account/Login']);
  }

  getDecodJWT(): any {
    return jwt_decode(this.getToken());
  }
}
