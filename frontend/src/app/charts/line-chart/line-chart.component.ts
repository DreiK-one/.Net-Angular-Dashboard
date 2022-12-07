import { Component, OnInit } from '@angular/core';
import { ChartData, ChartType, Colors } from 'chart.js';


const LINE_CHART_SAMPLE_DATA: any[] = [
  {data: [32, 14, 46, 23, 38, 56], label: 'Sentiment Analisis'},
  {data: [12, 18, 26, 13, 28, 26], label: 'Image Recognition'},
  {data: [52, 34, 49, 53, 68, 62], label: 'Forecasting'}
];

const LINE_CHART_LABELS: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'];

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {

  constructor() { }

  // lineChartData: ChartData<'line'> = {
  //   labels: LINE_CHART_LABELS,
  //   datasets: [
  //     {
  //       data: LINE_CHART_SAMPLE_DATA,
  //       backgroundColor: ['rgba(6, 214, 160, 0.2)', 'rgba(255, 209, 102, 0.2)','rgba(15, 78, 133, 0.2)',],
  //       borderColor: ['rgba(0, 200, 140, 0.5)', 'rgba(240, 180, 89, 0.5)', 'rgba(3, 64, 128, 0.5)'],
  //       pointBackgroundColor: ['#000', '#000', '#000'],
  //       pointBorderColor: ['#000', '#000', '#000'],
  //       pointHoverBackgroundColor: ['#555', '#555', '#555'],
  //       pointHoverBorderColor: ['#555', '#555', '#555']
  //     }
  //   ],
  // };

  lineChartData: ChartData<'line'> = {
      labels: LINE_CHART_LABELS,
      datasets: [
        {
          data: [32, 14, 46, 23, 38, 56],
          label: '123',
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
          data: [12, 18, 26, 13, 28, 26],
          label: '456',
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
          data: [52, 34, 49, 53, 68, 62],
          label: '789',
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
    };

  // lineChartData = LINE_CHART_SAMPLE_DATA;
  lineChartLabels = LINE_CHART_LABELS;
  lineChartOptions: any = {
    responsive: true,
  };
  lineChartLegend = true;
  lineChartType: ChartType = 'line';

  ngOnInit(): void {
  }

}
