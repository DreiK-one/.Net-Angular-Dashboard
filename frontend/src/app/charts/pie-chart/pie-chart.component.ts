import { Component, OnInit } from '@angular/core';
import { ChartData, ChartType } from 'chart.js';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {

  constructor() { }

  pieChartLabels: string[] = ['XYZ Logic', 'Main Bakery', 'Acme Hosting'];
  pieChartType: ChartType = 'doughnut';

  pieChartData: ChartData<'doughnut'> = {
    labels: this.pieChartLabels,
    datasets: [
      {
        data: [350, 450, 120],
        backgroundColor: ['#26547c', '#ff6b6b', '#ffd166'],
        borderColor: '#111'
      }
    ]
  };

  

  ngOnInit(): void {
  }

}
