import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  errorMessage: string;
  registerForm: FormGroup;
  jwt: string;

  constructor(private service: HttpService, private router: Router, private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      registerEmail: ["", [Validators.required, Validators.email]],
      registerPass: ["", Validators.required],
      registerPassConfirm: ["", Validators.required]
    });
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

  registerSend(email: string, pass: string, passconfirm: string) {
    this.service.registerPost(email, pass, passconfirm).subscribe(resualt => {
      this.jwt = resualt,
        localStorage.setItem('jwt', this.jwt),
        this.loggedOn()
    }, error => this.errorMessage = error['error']);
  }
}
