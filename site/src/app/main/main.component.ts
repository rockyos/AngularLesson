import { Component, OnInit, Input } from '@angular/core';
import { HttpService } from '../http.service';
import { Photo } from '../photo';
import { environment } from 'src/environments/environment.prod';
import { Router } from '@angular/router';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  @Input() photos: Photo[];
  url = `${environment.apiUrl}api/photo`;
  username: string;

  constructor(private service: HttpService, private router: Router) { }

  ngOnInit() {
    this.getData();
    const token: string = localStorage.getItem('jwt');
    var decoded = jwt_decode(token);
    this.username = decoded['sub'];
  }

  getData() {
    this.service.getPhotos().subscribe(resualt => this.photos = resualt);
  }

  btnLogOut(){
    localStorage.removeItem('jwt');
    this.router.navigate(['Account/Login']);
  }

  deleteBtn(photo: Photo) {
    let index = this.photos.indexOf(photo);
    this.service.delPhoto(photo).subscribe(response => this.photos.splice(index, 1));

  }
}
