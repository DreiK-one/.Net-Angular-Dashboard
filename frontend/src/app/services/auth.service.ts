import { Login } from './../shared/login.model';
import { User } from './../shared/user.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../shared/baseUrl';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _http: HttpClient) { }

  signUp(userObject: User){
    return this._http.post<any>(BASE_URL + 'User/register', userObject);
  }

  login(loginObject: Login){
    return this._http.post<any>(BASE_URL + 'User/authenticate', loginObject);
  }
}
