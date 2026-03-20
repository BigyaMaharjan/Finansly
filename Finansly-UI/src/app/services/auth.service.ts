import { Injectable, signal, computed, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import {
  Client,
  LoginRequestDto,
  RegisterRequestDto,
  ApiResponseOfAuthResponseDto,
} from '../api-client';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private client = inject(Client);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  private tokenKey = 'auth_token';

  private currentUser = signal<string | null>(null);

  isAuthenticated = computed(() => !!this.currentUser());

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem(this.tokenKey);
      if (token) {
        this.currentUser.set(token);
      }
    }
  }

  login(dto: LoginRequestDto): Promise<ApiResponseOfAuthResponseDto> {
    return new Promise((resolve, reject) => {
      this.client.login(dto).subscribe({
        next: (res) => {
          if (res.data?.token) {
            this.setToken(res.data.token);
            resolve(res);
          } else {
            reject(new Error('Invalid response'));
          }
        },
        error: (err) => reject(err),
      });
    });
  }

  register(dto: RegisterRequestDto): Promise<ApiResponseOfAuthResponseDto> {
    return new Promise((resolve, reject) => {
      this.client.register(dto).subscribe({
        next: (res) => resolve(res),
        error: (err) => reject(err),
      });
    });
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.tokenKey);
    }
    this.currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }

  private setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.tokenKey, token);
    }
    this.currentUser.set(token);
  }
}
