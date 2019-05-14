import { Injectable } from '@angular/core';
import { Photo } from './photo';
import { HttpClient} from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private url = "http://localhost:49508/" 
  constructor(private http: HttpClient) { }

  public getPhotos(): Observable<Array<Photo>>{
    return this.http.get<Array<Photo>>(this.url + "Photos/GetListfromDb");
  }

  public delPhoto(photo: Photo): Observable<string>{
    let data: any = {guid : photo.guid}
    return this.http.post<string>(this.url + "Delete", data);
  }
}
