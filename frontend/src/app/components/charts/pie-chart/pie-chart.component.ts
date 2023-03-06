import { Component, OnInit, Input } from '@angular/core';
import { ChartData, ChartType } from 'chart.js';
import _ from 'lodash';
import { THEME_COLORS } from 'src/app/shared/theme.colors';


const theme = 'Default';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {

  constructor() { }

  @Input() inputData: any;
  @Input() limit: any;

  chartData?: number[];
  pieChartLabels?: string[];
  pieChartType: ChartType = 'doughnut';
  pieChartData?: ChartData<'doughnut'>;

  ngOnInit(): void {
    this.parseChartData(this.inputData, this.limit);
  }

  parseChartData(res: any, limit?: number) {
    const allData = res.slice(0, limit);
    this.chartData = allData.map((x: any) => _.values(x)[1]);
    this.pieChartLabels = allData.map((x: any) => _.values(x)[0]);

    this.pieChartData = {
      labels: this.pieChartLabels,
      datasets: [
        {
          data: this.chartData as number[],
          backgroundColor: this.themeColors(theme),
          borderColor: '#111'
        }
      ]
    };
  }

  themeColors(setName: string): string[] {
    const colors = THEME_COLORS.slice(0)
      .find(set => set.name === setName)?.colorSet;

    return colors as string[];
  }
}
