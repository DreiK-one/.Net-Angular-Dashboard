import { SectionSalesComponent } from './components/sections/section-sales/section-sales.component';
import { SectionOrdersComponent } from './components/sections/section-orders/section-orders.component';
import { SectionHealthComponent } from './components/sections/section-health/section-health.component';
import { Routes } from "@angular/router";


export const appRoutes: Routes = [
    { path: 'sales', component: SectionSalesComponent },
    { path: 'orders', component: SectionOrdersComponent },
    { path: 'health', component: SectionHealthComponent },

    {path: '**', redirectTo: '/sales'}
];