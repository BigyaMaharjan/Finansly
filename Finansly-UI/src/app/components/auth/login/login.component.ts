import {
  Component,
  inject,
  ElementRef,
  AfterViewInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginRequestDto } from '../../../api-client';
import { AuthService } from '../../../services/auth.service';
import { animate, createTimeline } from 'animejs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements AfterViewInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private elementRef = inject(ElementRef);
  private loadingAnimation?: ReturnType<typeof animate>;

  @ViewChildren('loginCard') loginCard!: QueryList<ElementRef>;
  @ViewChildren('heading') heading!: QueryList<ElementRef>;
  @ViewChildren('emailGroup') emailGroup!: QueryList<ElementRef>;
  @ViewChildren('passwordGroup') passwordGroup!: QueryList<ElementRef>;
  @ViewChildren('loginBtn') loginBtn!: QueryList<ElementRef>;
  @ViewChildren('registerLink') registerLink!: QueryList<ElementRef>;
  @ViewChildren('errorBlock') errorBlock!: QueryList<ElementRef>;

  email = '';
  password = '';
  error = '';
  loading = false;

  private get nativeElement(): HTMLElement {
    return this.elementRef.nativeElement;
  }

  ngAfterViewInit(): void {
    this.animateEntrance();
  }

  private animateEntrance(): void {
    const card = this.loginCard.first?.nativeElement;
    const headingEl = this.heading.first?.nativeElement;
    const emailEl = this.emailGroup.first?.nativeElement;
    const passwordEl = this.passwordGroup.first?.nativeElement;
    const btnEl = this.loginBtn.first?.nativeElement;
    const linkEl = this.registerLink.first?.nativeElement;

    const tl = createTimeline({
      defaults: {
        ease: 'out(3)',
      },
    });

    tl.add(card, {
      translateY: [30, 0],
      opacity: [0, 1],
      duration: 600,
    })
      .add(
        headingEl,
        {
          opacity: [0, 1],
          duration: 400,
        },
        '-=300',
      )
      .add(
        emailEl,
        {
          translateX: [-20, 0],
          opacity: [0, 1],
          duration: 400,
        },
        '-=150',
      )
      .add(
        passwordEl,
        {
          translateX: [-20, 0],
          opacity: [0, 1],
          duration: 400,
        },
        '-=250',
      )
      .add(
        btnEl,
        {
          scale: [0.8, 1],
          opacity: [0, 1],
          duration: 400,
          easing: 'outBack',
        },
        '-=200',
      )
      .add(
        linkEl,
        {
          opacity: [0, 1],
          duration: 300,
        },
        '-=150',
      );
  }

  animateError(): void {
    const card = this.loginCard.first?.nativeElement;
    if (!card) return;

    animate(card, {
      translateX: [
        { to: -12, duration: 50 },
        { to: 12, duration: 50 },
        { to: -10, duration: 50 },
        { to: 10, duration: 50 },
        { to: -6, duration: 50 },
        { to: 6, duration: 50 },
        { to: 0, duration: 50 },
      ],
      ease: 'inOutQuad',
    });
  }

  animateLoading(): void {
    const btn = this.loginBtn.first?.nativeElement;
    if (!btn) return;

    this.loadingAnimation = animate(btn, {
      opacity: [1, 0.5, 1],
      duration: 1000,
      loop: true,
      ease: 'inOutSine',
    });
  }

  stopLoadingAnimation(): void {
    this.loadingAnimation?.pause();

    const btn = this.loginBtn.first?.nativeElement;
    if (btn) {
      btn.style.opacity = '1';
    }
  }

  onInputFocus(event: FocusEvent): void {
    const input = event.target as HTMLElement;

    animate(input, {
      scale: [1, 1.02],
      duration: 300,
      ease: 'outCubic',
    });
  }

  onInputBlur(event: FocusEvent): void {
    const input = event.target as HTMLElement;

    animate(input, {
      scale: [1.02, 1],
      duration: 300,
      ease: 'outCubic',
    });
  }

  onRipple(event: MouseEvent): void {
    const btn = event.currentTarget as HTMLElement;
    const ripple = document.createElement('span');
    ripple.classList.add('ripple');

    const rect = btn.getBoundingClientRect();
    const size = Math.max(rect.width, rect.height);
    ripple.style.width = ripple.style.height = `${size}px`;
    ripple.style.left = `${event.clientX - rect.left - size / 2}px`;
    ripple.style.top = `${event.clientY - rect.top - size / 2}px`;

    btn.appendChild(ripple);
    setTimeout(() => ripple.remove(), 600);
  }

  async onSubmit() {
    this.loading = true;
    this.error = '';
    this.animateLoading();

    const dto = new LoginRequestDto();
    dto.email = this.email;
    dto.password = this.password;

    try {
      await this.authService.login(dto);
      const card = this.loginCard.first?.nativeElement;
      await animate(card, {
        scale: [1, 1.05, 1],
        opacity: [1, 0],
        duration: 600,
        ease: 'inOutCubic',
      });

      this.router.navigate(['/']);
    } catch (err: any) {
      this.error = err?.error?.error?.message || 'Login failed';
      this.animateError();
    } finally {
      this.loading = false;
      this.stopLoadingAnimation();
    }
  }
}
