import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  photos: Photo[];
  
  url = `${environment.apiUrl}photo`;

  constructor(private service: HttpService) { }

  ngOnInit() {
    this.getData();
    console.log('onInit'); 
  }

  getData() {
    this.service.getPhotos().subscribe(resualt => {this.photos = resualt, console.log("gettt main: " + this.photos.length)});
  }

 
  deleteBtn(photo: Photo) {
    let index = this.photos.indexOf(photo);
    this.service.delPhoto(photo).subscribe(response => {this.photos.splice(index, 1), console.log("Array photos: " + this.photos.length)});
    
  }
}
