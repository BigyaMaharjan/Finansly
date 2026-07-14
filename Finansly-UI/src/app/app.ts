import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SessionTimeoutComponent } from './components/session-timeout/session-timeout.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SessionTimeoutComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('finansly-ui');
}
