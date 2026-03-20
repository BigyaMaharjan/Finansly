import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequestDto } from '../../../api-client';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  name = '';
  email = '';
  password = '';
  dateOfBirth: string = '';
  error = '';
  loading = false;

  async onSubmit() {
    this.loading = true;
    this.error = '';

    const dto = new RegisterRequestDto();
    dto.name = this.name;
    dto.email = this.email;
    dto.password = this.password;
    dto.dateOfBirth = this.dateOfBirth ? new Date(this.dateOfBirth) : undefined;

    try {
      await this.authService.register(dto);
      this.router.navigate(['/']);
    } catch (err: any) {
      this.error = err?.error?.error?.message || 'Registration failed';
    } finally {
      this.loading = false;
    }
  }
}
