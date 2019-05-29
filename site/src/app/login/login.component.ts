import { Component, OnInit } from '@angular/core';
import { Login } from '../login';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpService } from '../http.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup(
    {
      "userEmail": new FormControl("", [Validators.required, Validators.pattern("[a-zA-Z_]+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}")]),
      "userPassword": new FormControl("", Validators.required)
    }
  );
  constructor(private service: HttpService) { }

  ngOnInit() {

  }

  loginSend(email: string, pass: string) {
    this.service.loginPost(email, pass).subscribe();
  }
}
