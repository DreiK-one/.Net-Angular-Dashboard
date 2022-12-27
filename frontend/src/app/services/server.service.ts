import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { ServerMessage } from '../shared/server-message';
import { Server } from '../shared/server';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ServerService {

  constructor(private _http: HttpClient) { }

  getServers(): Observable<Server[]> {
    return this._http.get('https://localhost:7294/api/server')
    .pipe(
      catchError((error) => {
        return this.handleError(error);
      })
    )
    .pipe(res => res as Observable<Server[]>);
  }

  private handleError(error: any) {
    const errorMessage = (error.message) ? error.message : 
      error.status ? `${error.statusText} - ${error.statusText}` : 
      'Server error';

      return errorMessage;
  }
}
