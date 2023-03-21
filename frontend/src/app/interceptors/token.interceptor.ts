import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
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
            this._toastService.warning({detail: "Warning", summary: "Token is expired, please login again."});
            this._router.navigate(['login']);
          }
        }

        return throwError(() => new Error("Some other error occured"));
      })
    );
  }
}
