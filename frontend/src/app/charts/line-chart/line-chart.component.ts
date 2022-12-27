import { map } from 'rxjs';
import { SalesDataService } from './../../services/sales-data.service';
import { Component, OnInit } from '@angular/core';
import { ChartData, ChartType, Colors } from 'chart.js';
import moment from 'moment';
import { U } from 'chart.js/dist/chunks/helpers.core';


@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {

  constructor(private _salesDataService: SalesDataService) { }

  topCustomers?: string[];
  allOrders?: any[];
  lineChartData?: ChartData<'line'>;
  lineChartLabels?: string[];
  lineChartOptions: any = { responsive: true };
  lineChartLegend = true;
  lineChartType: ChartType = 'line';

  ngOnInit(): void {
    this._salesDataService.getOrders(1, 100).subscribe(res => {
      this.allOrders = (res as any)['page']['data'];

      this._salesDataService.getOrdersByCustomer(3).subscribe(cus => {
        this.topCustomers = (cus as any).map((x: any) => x['name']);

        const allChartData = this.topCustomers?.reduce((result: any, i) => {
          result.push(this.getChartData(this.allOrders, i));
          return result;
        }, []);

        let dates = allChartData.map( (x: any) => x['data']).reduce((a: any, i: any) => {
          a.push(i.map((o: any) => new Date(o[0])));
          return a;
        }, []);

        dates = [].concat.apply([], dates);

        const r = (this.getCustomerOrdersByDate(dates) as any)['data'];

        this.lineChartLabels = (r as any)[0]['orders'].map((o: any) => o['date']);

        this.lineChartData = {
          labels: this.lineChartLabels,
          datasets: [
            {
              data: (r as any)[0].orders.map((x: any) => x.total),
              label: (r as any)[0]['customer'],
              backgroundColor: 'rgba(6, 214, 160, 0.2)',
              borderColor: 'rgba(0, 200, 140, 0.5)',
              pointBackgroundColor: '#000',
              pointBorderColor: '#000',
              pointHoverBackgroundColor: '#555',
              pointHoverBorderColor: '#555',
              tension: 0.3,
              fill: true
            },
            {
              data: (r as any)[1].orders.map((x: any) => x.total),
              label: (r as any)[1]['customer'],
              backgroundColor: 'rgba(255, 209, 102, 0.2)',
              borderColor: 'rgba(240, 180, 89, 0.5)',
              pointBackgroundColor: '#000',
              pointBorderColor: '#000',
              pointHoverBackgroundColor: '#555',
              pointHoverBorderColor: '#555',
              tension: 0.3,
              fill: true
            },
            {
              data: (r as any)[2].orders.map((x: any) => x.total),
              label: (r as any)[2]['customer'],
              backgroundColor: 'rgba(15, 78, 133, 0.2)',
              borderColor: 'rgba(3, 64, 128, 0.5)',
              pointBackgroundColor: '#000',
              pointBorderColor: '#000',
              pointHoverBackgroundColor: '#555',
              pointHoverBorderColor: '#555',
              tension: 0.3,
              fill: true
            },
          ],
        }
      });
    });
  };

  getChartData(allOrders: any, name: string) {
    const customerOrders = allOrders.filter((o: any) => o.customer.name === name)

    const formattedOrders = customerOrders.reduce((r: any, e: any) => {
      r.push([e.placed, e.total]);
      return r;
    }, []);

    const result = {customer: name, data: formattedOrders};

    return result;
  };

  getCustomerOrdersByDate(dates: any) {
    const customers = this.topCustomers;
    const prettyDates = dates.map((x: any) => this.toFriendlyDates(x));
    const uniqueDates = Array.from(new Set(prettyDates)).sort();

    const result = {};
    const dataSets: any = (result as any)['data'] = [];

    customers?.reduce((x: any, y: any, i: any) => {
      const customerOrders: any[] = [];
      dataSets[i] = {
        customer: y, 
        orders: uniqueDates.reduce((r: any, e: any, j: any) => {
          const obj: any = {};
          obj['date'] = e;
          obj['total'] = this.getCustomerDateTotal(e, y);
          customerOrders.push(obj);

          return customerOrders;
        })
      };
      return x;
    }, []);

    return result;
  };

  toFriendlyDates(date: Date) {
    return moment(date).endOf('day').format('DD.MM.YY');
  };

  getCustomerDateTotal(date: any, customer: string) {
    const r = this.allOrders?.filter((o: any) => o.customer.name === customer 
      && this.toFriendlyDates(o.placed) === date);

    const result = (r as any).reduce((a: any, b: any) => {
      return a + b.total;
    }, 0);

    return result;
  };
}

