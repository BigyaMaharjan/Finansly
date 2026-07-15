import {
  Component,
  inject,
  AfterViewInit,
  ElementRef,
  QueryList,
  ViewChildren,
  signal,
} from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { animate, createTimeline } from 'animejs';

interface NavItem {
  path: string;
  label: string;
}

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements AfterViewInit {
  @ViewChildren('navItem') navItems!: QueryList<ElementRef>;

  navItemsData: NavItem[] = [
    { path: '/categories', label: 'Categories' }
  ];

  // State for mobile menu
  isMobileMenuOpen = signal(false);

  ngAfterViewInit(): void {
    this.animateNavbar();
  }

  private animateNavbar(): void {
    const navbar = this.getNavbarElement();
    if (!navbar) return;

    // Staggered animation for nav items
    const items = this.navItems.toArray();
    if (items.length > 0) {
      const tl = createTimeline({
        defaults: {
          ease: 'out(3)',
        },
      });

      items.forEach((item, index) => {
        tl.add(item.nativeElement, {
          opacity: [0, 1],
          translateX: [-20, 0],
          duration: 300,
        }, `-=${300 - index * 50}`);
      });
    }
  }

  private getNavbarElement(): HTMLElement {
    const navLinks = this.navItems.first?.nativeElement?.parentElement;
    return navLinks?.parentElement as HTMLElement;
  }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen.update(prev => !prev);
  }

  closeMobileMenu(): void {
    this.isMobileMenuOpen.set(false);
  }
}