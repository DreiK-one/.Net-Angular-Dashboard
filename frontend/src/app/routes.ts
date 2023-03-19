import { MainLayoutComponent } from './shared/main-layout/main-layout.component';
import { AuthLayoutComponent } from './shared/auth-layout/auth-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { SignupComponent } from './components/signup/signup.component';
import { LoginComponent } from './components/login/login.component';
import { SectionSalesComponent } from './components/sections/section-sales/section-sales.component';
import { SectionOrdersComponent } from './components/sections/section-orders/section-orders.component';
import { SectionHealthComponent } from './components/sections/section-health/section-health.component';
import { Routes } from "@angular/router";


export const appRoutes: Routes = [
    { path: '', component: AuthLayoutComponent, children: [
        { path: '', redirectTo: '/sales', pathMatch: 'full'},
        { path: 'login', component: LoginComponent},
        { path: 'signup', component: SignupComponent},

    ]},
    { path: '', component: MainLayoutComponent, children: [
        { path: 'sales', component: SectionSalesComponent, canActivate: [AuthGuard] },
        { path: 'orders', component: SectionOrdersComponent, canActivate: [AuthGuard] },
        { path: 'health', component: SectionHealthComponent, canActivate: [AuthGuard] },
    ]},

    {path: '**', redirectTo: '/sales'}
];