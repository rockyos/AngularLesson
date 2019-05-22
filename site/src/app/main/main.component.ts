import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  @Input() photos: Photo[];
  url = `${environment.apiUrl}photo`;

  constructor(private service: HttpService) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.service.getPhotos().subscribe(resualt => this.photos = resualt);
  }

 
  deleteBtn(photo: Photo) {
    let index = this.photos.indexOf(photo);
    this.service.delPhoto(photo).subscribe(response => this.photos.splice(index, 1));
    
  }
}
