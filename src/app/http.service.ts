import { Injectable } from '@angular/core';
import { Photo } from './photo';
import { HttpClient} from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private url = "https://localhost:44302/api/photo"; 
  constructor(private http: HttpClient) { }

  public getPhotos(): Observable<Array<Photo>>{
    return this.http.get<Array<Photo>>(this.url);
  }

  public addPhoto(Images: File): Observable<File>{
    const formData = new FormData();
    formData.append('file', Images);
    return this.http.post<File>(this.url, formData);
  }

  public delPhoto(photo: Photo): Observable<any>{
    //let data: any = {guid : photo.guid};
    return this.http.delete(this.url + "/" + photo.guid);
  }
}
