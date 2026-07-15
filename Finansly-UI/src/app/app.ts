import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SessionTimeoutComponent } from './components/session-timeout/session-timeout.component';
import { NavbarComponent } from './components/navbar/navbar.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SessionTimeoutComponent, NavbarComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('finansly-ui');

  private router = inject(Router);

  showNavbar = computed(() => {
    const url = this.router.url;
    return !url.startsWith('/auth');
  });
}
