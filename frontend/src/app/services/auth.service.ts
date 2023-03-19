import { Router } from '@angular/router';
import { Login } from './../shared/login.model';
import { User } from './../shared/user.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BASE_URL } from '../shared/baseUrl';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _http: HttpClient,
    private _router: Router) { }

  signUp(userObject: User){
    return this._http.post<any>(BASE_URL + 'User/register', userObject);
  }

  signout(){
    localStorage.clear();
    this._router.navigate(['login']);
  }

  login(loginObject: Login){
    return this._http.post<any>(BASE_URL + 'User/authenticate', loginObject);
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token');
  }
}
