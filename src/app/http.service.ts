import { Injectable } from '@angular/core';
import { Photo } from './photo';
import { HttpClient} from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private url = "http://localhost:2403/"
  constructor(private http: HttpClient) { }

  public getPhotos(): Observable<Array<Photo>>{
    return this.http.get<Array<Photo>>(this.url + "Photos/GetListfromDb");
  }

  public delPhoto(photo: Photo): Observable<Photo>{
    return this.http.post<Photo>(this.url, photo.guid);
  }
}
