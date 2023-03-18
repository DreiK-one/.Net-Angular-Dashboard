import { AuthGuard } from './guards/auth.guard';
import { SignupComponent } from './components/signup/signup.component';
import { LoginComponent } from './components/login/login.component';
import { SectionSalesComponent } from './components/sections/section-sales/section-sales.component';
import { SectionOrdersComponent } from './components/sections/section-orders/section-orders.component';
import { SectionHealthComponent } from './components/sections/section-health/section-health.component';
import { Routes } from "@angular/router";


export const appRoutes: Routes = [
    { path: 'login', component: LoginComponent},
    { path: 'signup', component: SignupComponent},
    { path: 'sales', component: SectionSalesComponent, canActivate: [AuthGuard] },
    { path: 'orders', component: SectionOrdersComponent, canActivate: [AuthGuard] },
    { path: 'health', component: SectionHealthComponent, canActivate: [AuthGuard] },

    {path: '**', redirectTo: '/sales'}
];