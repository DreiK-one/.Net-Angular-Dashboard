import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/order';
import { SalesDataService } from '../../services/sales-data.service';


@Component({
  selector: 'app-section-orders',
  templateUrl: './section-orders.component.html',
  styleUrls: ['./section-orders.component.css']
})
export class SectionOrdersComponent implements OnInit {

  constructor(private _salesData: SalesDataService) { }

  orders: any;
  total = 0;
  page = 1;
  limit = 10;

  ngOnInit() {
    this.getOrders();
  }

  getOrders(): void {
    this._salesData.getOrders(this.page, this.limit)
      .subscribe(res => {
        this.orders = res;
      });
  }

}
