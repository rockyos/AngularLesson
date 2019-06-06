import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpService } from '../http.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {
  errorMessage: string;
  resetPassForm: FormGroup;
  code: string;

  constructor(private service: HttpService, private activateRoute: ActivatedRoute, private fb: FormBuilder) {
    this.code = this.activateRoute.snapshot.queryParamMap.get("code");
    console.log(this.code);
    this.code = this.code.replace(/\s+/g, '+');
    console.log(this.code);
    this.resetPassForm = this.fb.group({
      resEmail: ["", [Validators.required, Validators.email]],
      resPass: ["", Validators.required],
      resPassConfirm: ["", Validators.required]
    });
  }

  ngOnInit() {
  }

  resetPassSend(email: string, pass: string, passconfirm: string){
    this.service.resetPassPost(email, pass, passconfirm, this.code).subscribe(resualt => {
      this.errorMessage =  resualt['value']['message']});
  }

}
