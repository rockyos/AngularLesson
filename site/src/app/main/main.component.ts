import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';
import { BlockingProxy } from 'blocking-proxy';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  photos: Photo[];
  file: File;
  hiddenIcon: boolean = false;
  url = `${environment.apiUrl}photo`;

  constructor(private service: HttpService) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.service.getPhotos().subscribe(resualt => this.photos = resualt);
  }

  addPhoto(fileInput: any) {
    const newFile = fileInput.target.files[0] as File;
    this.service.addPhoto(newFile).subscribe(response => this.getData());
  }

  saveBtn() {
    this.service.save().subscribe();
  }

  resetBtn() {
    this.service.reset().subscribe();
  }

  deleteBtn(photo: Photo) {
    let index = this.photos.indexOf(photo);
    this.service.delPhoto(photo).subscribe(response => this.photos.splice(index, 1));
  }
}
