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
    formData.append('newImage', newImage, newImage.name);
    const url = `${environment.apiUrl}photo/send`;
    return this.http.post<string>(url, formData);
  }

  public save(): Observable<any>{
    const url = `${environment.apiUrl}photo/save`;
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<any>(url, httpOptions);
  }

  public reset(): Observable<any>{
    const url = `${environment.apiUrl}photo/reset`;
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<any>(url, httpOptions);
  }

  public delPhoto(photo: Photo): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    const url = `${environment.apiUrl}photo/${photo.guid}`;
    return this.http.delete(url);
  }
}
