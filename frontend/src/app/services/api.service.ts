import { BASE_URL } from './../shared/baseUrl';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private _http: HttpClient) { }

  getUsers(){
    return this._http.get<any>(BASE_URL + 'User');
  }
}
