import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServerMessage } from '../shared/server-message';
import { Server } from '../shared/server';
import { catchError } from 'rxjs/operators';
import { BASE_URL } from '../shared/baseUrl';


@Injectable({
  providedIn: 'root'
})
export class ServerService {

  constructor(private _http: HttpClient) { 
    this.options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json', 
        'Accept': 'q=0.8;application/json; q=0.9'
      })
    }
  }

  options:any;

  getServers(): Observable<Server[]> {
    return this._http.get(BASE_URL + 'server')
    .pipe(
      catchError((error) => {
        return this.handleError(error);
      })
    )
    .pipe(res => res as Observable<Server[]>);
  }

  handleServerMessage(message: ServerMessage): Observable<Response> {
    const url = BASE_URL + 'server/' + message.id;

    return this._http.put(url, message, this.options).pipe((res: any) => res);
  }

  private handleError(error: any) {
    const errorMessage = (error.message) ? error.message : 
      error.status ? `${error.statusText} - ${error.statusText}` : 
      'Server error';

      return errorMessage;
  }
}
