import {
  Component,
  AfterViewInit,
  ElementRef,
  QueryList,
  ViewChildren,
  signal,
  OnDestroy,
  HostListener,
} from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { animate, stagger } from 'animejs';

interface NavItem {
  path: string;
  label: string;
  icon: string; // material icon name
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements AfterViewInit, OnDestroy {
  @ViewChildren('navItem') navItems!: QueryList<ElementRef>;

  navItemsData: NavItem[] = [
    { path: '/categories', label: 'Categories', icon: 'category' },
    // add more as you add/build screens
  ];

  // State for mobile menu
  isMobileMenuOpen = signal(false);

  ngAfterViewInit(): void {
    this.animateNavItems();
  }

  ngOnDestroy(): void {
    document.body.style.overflow = '';
  }

  private animateNavItems(): void {
    // Staggered animation for nav items
    const items = this.navItems.toArray().map((el) => el.nativeElement);
    if (!items.length) return;

    animate(items, {
      opacity: [0, 1],
      translateX: [-20, 0],
      duration: 300,
      delay: stagger(50),
      ease: 'out(3)',
    });
  }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen.update((v) => !v);
    document.body.style.overflow = this.isMobileMenuOpen() ? 'hidden' : '';
  }

  closeMobileMenu(): void {
    this.isMobileMenuOpen.set(false);
    document.body.style.overflow = '';
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    if (this.isMobileMenuOpen()) this.closeMobileMenu();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.isMobileMenuOpen()) return;
    const target = event.target as HTMLElement;

    // close if click lands outside the nav entirely
    if (!target.closest('app-navbar')) {
      this.closeMobileMenu();
    }
  }
}
