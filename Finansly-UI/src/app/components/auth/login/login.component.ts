import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginRequestDto } from '../../../api-client';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';
  error = '';
  loading = false;

  async onSubmit() {
    this.loading = true;
    this.error = '';

    const dto = new LoginRequestDto();
    dto.email = this.email;
    dto.password = this.password;

    try {
      await this.authService.login(dto);
      this.router.navigate(['/']);
    } catch (err: any) {
      this.error = err?.error?.error?.message || 'Login failed';
    } finally {
      this.loading = false;
    }
  }
}
