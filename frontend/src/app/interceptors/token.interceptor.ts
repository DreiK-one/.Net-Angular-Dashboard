import { Router } from '@angular/router';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NgToastService } from 'ng-angular-popup';
import { TokenModel } from '../shared/token.model';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private _authService: AuthService, 
    private _toastService: NgToastService,
    private _router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this._authService.getToken();

    if (myToken) {
      request = request.clone({
        setHeaders: {Authorization: `Bearer ${myToken}`}
      })
    }

    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            // this._toastService.warning({detail: "Warning", summary: "Token is expired, please login again."});
            // this._router.navigate(['login']);
            return this.handleUnAuthorizedError(request, next);
          }
        }

        return throwError(() => this._toastService.error({detail: "ERROR", summary: err.error.message, duration: 5000}));
      })
    );
  }

  handleUnAuthorizedError(request: HttpRequest<any>, next: HttpHandler){
    let tokenModel = new TokenModel();
    tokenModel.accessToken = this._authService.getToken()!;
    tokenModel.refreshToken = this._authService.getRefreshToken()!;

    return this._authService.renewToken(tokenModel)
      .pipe(
        switchMap((data: TokenModel) => {
          this._authService.storeRefreshToken(data.refreshToken);
          this._authService.storeToken(data.accessToken);

          request = request.clone({
            setHeaders: {Authorization: `Bearer ${data.accessToken}`}
          });

          return next.handle(request);
        }),
        catchError((err) => {
          return throwError(() => {
            this._toastService.warning({detail: "Warning", summary: "Token is expired, please login again."});
            this._router.navigate(['login']);
          })
        })
      )
  }
}
