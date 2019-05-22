import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { MainComponent } from '../main/main.component'

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {

  hiddenIcon: boolean = false;

  constructor(private service: HttpService, private main: MainComponent) { }

  ngOnInit() {
  }

  addPhoto(fileInput: any) {
    const newFile = fileInput.target.files[0] as File;
    this.service.addPhoto(newFile).subscribe(response => this.main.getData());
  }

  saveBtn() {
    this.service.save().subscribe(response => this.main.getData());
  }

  resetBtn() {
    this.service.reset().subscribe(response => {this.main.getData(), console.log("gettt form")});
  }

}
