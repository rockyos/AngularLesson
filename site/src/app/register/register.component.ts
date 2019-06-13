import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { TokenService } from '../token.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  errorMessage: string;
  registerForm: FormGroup;
  jwt: string;

  constructor(private service: HttpService, private token: TokenService, private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      registerEmail: ["", [Validators.required, Validators.email]],
      registerPass: ["", Validators.required],
      registerPassConfirm: ["", Validators.required]
    });
  }

  ngOnInit() {
    this.token.loggedOn();
  }

  registerSend(email: string, pass: string, passconfirm: string) {
    this.token.sessionOrLocalStorage(false);
    this.service.registerPost(email, pass, passconfirm).subscribe(resualt => {
      this.jwt = resualt,
        this.token.setToken(this.jwt),
        this.token.loggedOn()
    }, error => this.errorMessage = error['error']);
  }
}
