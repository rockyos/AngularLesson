import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpService } from '../http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-external',
  templateUrl: './register-external.component.html',
  styleUrls: ['./register-external.component.css']
})
export class RegisterExternalComponent implements OnInit {

  errorMessage: string;
  registerextForm: FormGroup;

  constructor(private service: HttpService, private router: Router, private fb: FormBuilder) {
    this.registerextForm = this.fb.group({
      registerextEmail: ["", [Validators.required, Validators.email]]
    });
  }

  ngOnInit() {
  }

  registerExtSend(email: string) {
    this.service.registerExtPost(email).subscribe(resualt =>
      this.router.navigate(['']),
      error => this.errorMessage = error['error']
    );
  }

}
