import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpService } from '../http.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  returnUrl: string;
  url = `${environment.apiUrl}`;
  loginForm: FormGroup = new FormGroup(
    {
      "userEmail": new FormControl("", [Validators.required, Validators.pattern("[a-zA-Z_]+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}")]),
      "userPassword": new FormControl("", Validators.required),
      "userCheckbox": new FormControl()
    }
  );
  constructor(private service: HttpService, private activateRoute: ActivatedRoute) {
    this.returnUrl = this.activateRoute.snapshot.queryParamMap.get("ReturnUrl");
  }

  ngOnInit() {

  }

  loginSend(email: string, pass: string, checked: any) {
    this.service.loginPost(email, pass, checked, this.returnUrl).subscribe();
  }
}
