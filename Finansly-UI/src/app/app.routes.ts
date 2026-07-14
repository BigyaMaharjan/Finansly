import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { Category } from './components/category/category';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'categories', component: Category, canActivate: [authGuard] },
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },
];