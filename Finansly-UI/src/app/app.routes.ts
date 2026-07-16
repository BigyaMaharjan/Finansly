import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { Category } from './components/category/category';
import { authGuard } from './guards/auth.guard';
import { NavbarComponent } from './components/navbar/navbar.component';

export const routes: Routes = [
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  {
    path: '',
    component: NavbarComponent,        // has <app-navbar> + <router-outlet>
    canActivate: [authGuard],
    children: [
      { path: 'categories', component: Category },
      { path: '', redirectTo: 'categories', pathMatch: 'full' },
      // ...other authenticated pages
    ],
  },
  { path: '**', redirectTo: '/auth/login'},
];