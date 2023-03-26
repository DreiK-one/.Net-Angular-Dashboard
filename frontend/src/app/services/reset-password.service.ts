import { BASE_URL } from './../shared/baseUrl';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResetPasswordModel } from '../shared/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {

  constructor(private _httpClient: HttpClient) { }

  sendResetPasswordLink(email: string){
    return this._httpClient.post<any>(`${BASE_URL}User/send-reset-email/${email}`, {});
  }

  resetPassword(resetPasswordObject: ResetPasswordModel){
    return this._httpClient.post<any>(`${BASE_URL}/User/reset-password`, resetPasswordObject);
  }
}
