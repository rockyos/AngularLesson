import { Component } from '@angular/core';
import { Photo } from '../app/photo';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'AngularLesson';

  photos: Photo[];

  public photosChanges(images){
    this.photos = images;
  }

}
