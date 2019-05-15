import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  photos: Photo[];
  file: File;
  constructor(private service: HttpService) { }

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

  deleteBtn(photo: Photo){
    let index = this.photos.indexOf(photo);
    this.photos.splice(index,1);
    this.service.delPhoto(photo).subscribe();
  }
}
