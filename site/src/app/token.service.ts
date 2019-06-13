import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private router: Router) { }

  tokenKey: string = 'jwt';

  loggedOn() {
    const token: string = localStorage.getItem(this.tokenKey);
    if (token) {
      this.router.navigate(['']);
    }
  }

  setToken(jwt: string) {
    localStorage.setItem(this.tokenKey, jwt);
  }

  getToken(): string {
    return localStorage.getItem(this.tokenKey);
  }

  logOut() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['Account/Login']);
  }

  getDecodJWT(): any {
    return jwt_decode(this.getToken());
  }
}
