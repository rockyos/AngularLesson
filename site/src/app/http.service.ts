import { Injectable } from '@angular/core';
import { Photo } from './photo';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(private http: HttpClient) { }

  public getPhotos(): Observable<Array<Photo>> {
    const url = `${environment.apiUrl}photo`;
    return this.http.get<Array<Photo>>(url);
  }

  public addPhoto(newImage: File): Observable<string> {
    const formData = new FormData();
    console.log(newImage);
    formData.append('newImage', newImage, newImage.name);
    const url = `${environment.apiUrl}photo`;
    return this.http.post<string>(url, formData);
  }

  // public save(): Observable<any>{
  //   return this.http.post<any>(this.url, "save");
  // }

  // public reset(): Observable<any>{
  //   return this.http.post<any>(this.url, "reset");
  // }

  public delPhoto(photo: Photo): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    const data: any = { id: photo.guid };
    const url = `${environment.apiUrl}photo/${data}`;
    return this.http.delete(url);
  }
}
