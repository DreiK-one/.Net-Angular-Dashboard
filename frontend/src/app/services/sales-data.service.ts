import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { BASE_URL } from '../shared/baseUrl';


@Injectable({
  providedIn: 'root'
})
export class SalesDataService {

  constructor(private _http: HttpClient) { }


  getOrders(pageIndex: number, pageSize: number) {
    return this._http.get(BASE_URL + 'order/' + pageIndex + '/' + pageSize)
      .pipe(map(res => res || []));
  }

  getOrdersByCustomer(n: number) {
    return this._http.get(BASE_URL + 'order/by-customer/' + n)
      .pipe(map(res => res || []));
  }

  getOrdersByState() {
    return this._http.get(BASE_URL + 'order/by-state/')
      .pipe(map(res => res || []));
  }
}
