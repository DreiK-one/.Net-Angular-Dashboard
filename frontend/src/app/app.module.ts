import { TokenInterceptor } from './interceptors/token.interceptor';
import { appRoutes } from './routes';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SectionSalesComponent } from './components//sections/section-sales/section-sales.component';
import { SectionOrdersComponent } from './components//sections/section-orders/section-orders.component';
import { SectionHealthComponent } from './components/sections/section-health/section-health.component';
import { BarChartComponent } from './components/charts/bar-chart/bar-chart.component';
import { LineChartComponent } from './components/charts/line-chart/line-chart.component';
import { PieChartComponent } from './components/charts/pie-chart/pie-chart.component';
import { ServerComponent } from './components/server/server.component';

import { SalesDataService } from './services/sales-data.service';
import { ServerService } from './services/server.service';
import { PaginationComponent } from './components/pagination/pagination.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgToastModule } from 'ng-angular-popup';
import { AuthLayoutComponent } from './shared/auth-layout/auth-layout.component';
import { MainLayoutComponent } from './shared/main-layout/main-layout.component';
import { SectionUsersComponent } from './components/sections/section-users/section-users.component';
import { FormsModule } from '@angular/forms';
import { ResetComponent } from './components/reset/reset.component';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    SectionSalesComponent,
    SectionOrdersComponent,
    SectionHealthComponent,
    BarChartComponent,
    LineChartComponent,
    PieChartComponent,
    ServerComponent,
    PaginationComponent,
    LoginComponent,
    SignupComponent,
    AuthLayoutComponent,
    MainLayoutComponent,
    SectionUsersComponent,
    ResetComponent
  ],
  imports: [
    BrowserModule, 
    RouterModule.forRoot(appRoutes),
    NgChartsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgToastModule,
    FormsModule
  ],
  providers: [
    SalesDataService, 
    ServerService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
