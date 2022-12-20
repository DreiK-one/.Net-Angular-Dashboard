import { SalesDataService } from './../../services/sales-data.service';
import { Component, OnInit } from '@angular/core';
import { ChartType } from 'chart.js';
import * as moment from 'moment';


@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit {

  constructor(private _salesDataService: SalesDataService) { }

  orders: any;
  orderLabels?: string[];
  orderData?: number[];

  public barChartData?: any[];
  public barChartLabels?: string[];
  public barChartType: ChartType = 'bar';
  public barChartLegend: boolean = true;
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  }

  ngOnInit(): void {
    this._salesDataService.getOrders(1, 100)
      .subscribe(res => {
        const localChartData = this.getChartData(res as Response);
        this.barChartLabels = localChartData.map((x: any) => x[0]).reverse();
        this.barChartData = [{'data': localChartData.map((x: any) => x[1]), 'label': 'Sales'}];
      })
  }

  getChartData(res: Response) {
    this.orders = (res as any)['page']['data'];
    const data = this.orders.map((o: any) => o.total);

    const formattedOrders = this.orders.reduce((r: any, e: any) => {
      r.push([moment(e.placed).format('DD.MM.YY'), e.total]);
      return r;
    }, []);

    const p: any[] = [];

    const chartData = formattedOrders.reduce((r: any, e: any) => {
      const key = e[0];
      if (!p[key]) {
        p[key] = e;
        r.push(p[key]);
      } else {
        p[key][1] += e[1];
      }

      return r;
    }, []);

    return chartData;
  }

}
