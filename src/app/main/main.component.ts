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
  constructor(private service: HttpService) { }

  ngOnInit() {
    this.getData();
  }

  getData(){
    this.service.getPhotos().subscribe(resualt => { this.photos = resualt, console.log(this.photos)});
  }

  deleteBtn(photo: Photo){
    let index = this.photos.indexOf(photo);
    this.photos.splice(index,1);
    this.service.delPhoto(photo).subscribe();
  }
}
