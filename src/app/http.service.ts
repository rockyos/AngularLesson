import { Injectable } from '@angular/core';
import { Photo } from './photo';
import { HttpClient, HttpHeaders} from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  public url = "https://localhost:44302/api/photo"; 
  constructor(private http: HttpClient) { }

  public getPhotos(): Observable<Array<Photo>>{
    return this.http.get<Array<Photo>>(this.url);
  }

  public addPhoto(Images: File): Observable<File>{
    const httpOptions ={
      headers: new HttpHeaders({'Content-Type' : 'application/json'})
    };
    const formData = new FormData();
    formData.append('file', Images);
    return this.http.post<File>(this.url, formData, httpOptions);
  }

  // public save(): Observable<any>{
  //   return this.http.post<any>(this.url, "save");
  // }

  // public reset(): Observable<any>{
  //   return this.http.post<any>(this.url, "reset");
  // }

  public delPhoto(photo: Photo): Observable<any>{
    const httpOptions ={
      headers: new HttpHeaders({'Content-Type' : 'application/json'})
    };
    let data: any = {id : photo.guid};
    return this.http.delete(this.url + "/" + data);
  }
}
