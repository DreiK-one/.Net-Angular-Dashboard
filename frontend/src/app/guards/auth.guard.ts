import { NgToastService } from 'ng-angular-popup';
import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private _authService: AuthService, 
    private _router: Router,
    private _toast: NgToastService) { }

  canActivate(): boolean {
    if(this._authService.isLoggedIn()){
      return true;
    }
    this._toast.error({detail: 'ERROR', summary: 'You must be login first!'})
    this._router.navigate(['login']);
    return false;
  }
  
}
