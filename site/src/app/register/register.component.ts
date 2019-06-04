import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private service: HttpService) {

   }
  registerForm: FormGroup = new FormGroup(
    {
      "registerEmail": new FormControl("", [Validators.required, Validators.pattern("[a-zA-Z_]+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}")]),
      "registerPass": new FormControl("", Validators.required),
      "registerPassConfirm": new FormControl("", Validators.required)
    }
  );

  ngOnInit() {
  }

  registerSend(email: string, password: string, confirmpassword: string){
    this.service.registerPost(email, password, confirmpassword).subscribe();
  }
}
