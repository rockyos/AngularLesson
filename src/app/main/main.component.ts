import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';
import { BlockingProxy } from 'blocking-proxy';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  photos: Photo[];
  file: File;
  hiddenIcon: boolean = false;
  constructor(private service: HttpService) { }
  
  url = this.service.url;

  ngOnInit() {
    this.getData();
  }

  getData(){
    this.service.getPhotos().subscribe(resualt =>  this.photos = resualt);
  }

  addPhoto(event){
    this.file = event.target || event.srcElement;
    this.service.addPhoto(this.file).subscribe(response => this.getData());
  }

  saveBtn(){
    //this.service.save().subscribe();
    console.log("save");
  }

  resetBtn(){
     //this.service.reset().subscribe();
    console.log("reset");
  }

  deleteBtn(photo: Photo){
    let index = this.photos.indexOf(photo);
    this.service.delPhoto(photo).subscribe(response => this.photos.splice(index,1));
  }
}
