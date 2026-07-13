import {
  Component,
  inject,
  signal,
  ElementRef,
  AfterViewInit,
  QueryList,
  ViewChildren,
  OnInit,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import {
  CategoryDto,
  CategoryTypeLookupDto,
  CreateCategoryDto,
  UpdateCategoryDto,
  CreateCategoryWithTransactionsDto,
  CreateTransactionForCategoryDto,
} from '../../api-client';
import { CategoryService } from '../../services/category.service';
import { animate, stagger } from 'animejs';

interface TransactionRow {
  amount: number | null;
  date: string;
  description: string;
}

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [FormsModule, DatePipe],
  templateUrl: './category.html',
  styleUrl: './category.scss',
})
export class Category implements OnInit, AfterViewInit {
  private categoryService = inject(CategoryService);
  private elementRef = inject(ElementRef);

  @ViewChildren('pageHeader') pageHeader!: QueryList<ElementRef>;
  @ViewChildren('categoryCard') categoryCards!: QueryList<ElementRef>;
  @ViewChildren('modalCard') modalCard!: QueryList<ElementRef>;

  categories = signal<CategoryDto[]>([]);
  categoryTypes = signal<CategoryTypeLookupDto[]>([]);
  loading = signal(true);
  error = signal('');

  showModal = false;
  editingCategory: CategoryDto | null = null;
  modalLoading = false;
  modalError = '';
  modalName = '';
  modalType: number | null = null;

  confirmDeleteId: string | null = null;

  includeTransactions = false;
  transactions: TransactionRow[] = [];

  ngOnInit(): void {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.animateEntrance();
  }

  async loadData(): Promise<void> {
    this.loading.set(true);
    this.error.set('');
    try {
      const [cats, types] = await Promise.all([
        this.categoryService.getAll(),
        this.categoryService.getTypes(),
      ]);
      this.categories.set(cats);
      this.categoryTypes.set(types);
    } catch (err: any) {
      this.error.set(err?.error?.error?.message || 'Failed to load categories. Please try again.');
    } finally {
      this.loading.set(false);
      setTimeout(() => this.animateEntrance(), 50);
    }
  }

  openCreateModal(): void {
    this.editingCategory = null;
    this.modalName = '';
    this.modalType = null;
    this.modalError = '';
    this.includeTransactions = false;
    this.transactions = [];
    this.showModal = true;
    setTimeout(() => this.animateModal(), 50);
  }

  openEditModal(cat: CategoryDto): void {
    this.editingCategory = cat;
    this.modalName = cat.name ?? '';
    this.modalType = cat.type ?? null;
    this.modalError = '';
    this.includeTransactions = false;
    this.transactions = [];
    this.showModal = true;
    setTimeout(() => this.animateModal(), 50);
  }

  closeModal(): void {
    const card = this.modalCard.first?.nativeElement;
    if (card) {
      animate(card, {
        scale: [1, 0.9],
        opacity: [1, 0],
        duration: 200,
        ease: 'inCubic',
      }).then(() => {
        this.showModal = false;
        this.editingCategory = null;
        this.modalError = '';
      });
    } else {
      this.showModal = false;
      this.editingCategory = null;
      this.modalError = '';
    }
  }

  async submitModal(): Promise<void> {
    if (!this.modalName.trim() || this.modalType === null) {
      this.modalError = 'Name and type are required.';
      this.animateError();
      return;
    }

    this.modalLoading = true;
    this.modalError = '';

    try {
      if (this.editingCategory) {
        const dto = new UpdateCategoryDto();
        dto.name = this.modalName.trim();
        dto.type = this.modalType;
        await this.categoryService.update(this.editingCategory.id!, dto);
      } else if (this.includeTransactions && this.transactions.length > 0) {
        const txDtos = this.transactions
          .filter((t) => t.amount !== null)
          .map((t) => {
            const dto = new CreateTransactionForCategoryDto();
            dto.amount = t.amount!;
            dto.date = t.date ? new Date(t.date) : undefined;
            dto.description = t.description || undefined;
            return dto;
          });

        const composite = new CreateCategoryWithTransactionsDto();
        composite.name = this.modalName.trim();
        composite.type = this.modalType;
        composite.transactions = txDtos;
        await this.categoryService.createWithTransactions(composite);
      } else {
        const dto = new CreateCategoryDto();
        dto.name = this.modalName.trim();
        dto.type = this.modalType;
        await this.categoryService.create(dto);
      }

      this.showModal = false;
      this.editingCategory = null;
      await this.loadData();
    } catch (err: any) {
      this.modalError = err?.error?.error?.message || 'Operation failed. Please try again.';
      this.animateError();
    } finally {
      this.modalLoading = false;
    }
  }

  confirmDelete(id: string): void {
    this.confirmDeleteId = id;
  }

  cancelDelete(): void {
    this.confirmDeleteId = null;
  }

  async deleteCategory(id: string): Promise<void> {
    const cards = this.categoryCards.toArray();
    const idx = this.categories().findIndex((c: CategoryDto) => c.id === id);
    const cardEl = cards[idx]?.nativeElement;

    if (cardEl) {
      await new Promise<void>((resolve) => {
        animate(cardEl, {
          scale: [1, 0.8],
          opacity: [1, 0],
          duration: 300,
          ease: 'inBack',
        }).then(() => resolve());
      });
    }

    try {
      await this.categoryService.delete(id);
      this.categories.update((cats: CategoryDto[]) => cats.filter((c: CategoryDto) => c.id !== id));
      this.confirmDeleteId = null;
    } catch {
      this.confirmDeleteId = null;
      this.loadData();
    }
  }

  addTransaction(): void {
    this.transactions.push({ amount: null, date: '', description: '' });
  }

  removeTransaction(index: number): void {
    this.transactions.splice(index, 1);
  }

  private animateEntrance(): void {
    const header = this.pageHeader.first?.nativeElement;
    if (header) {
      animate(header, {
        translateY: [-30, 0],
        opacity: [0, 1],
        duration: 500,
        ease: 'out(3)',
      });
    }

    const cards = this.categoryCards.toArray().map((c) => c.nativeElement);
    if (cards.length > 0) {
      animate(cards, {
        translateY: [20, 0],
        opacity: [0, 1],
        scale: [0.95, 1],
        duration: 400,
        delay: stagger(80),
        ease: 'out(3)',
      });
    }
  }

  private animateModal(): void {
    const card = this.modalCard.first?.nativeElement;
    if (!card) return;

    animate(card, {
      scale: [0.9, 1],
      opacity: [0, 1],
      translateY: [30, 0],
      duration: 400,
      ease: 'outBack',
    });
  }

  animateError(): void {
    const card = this.modalCard.first?.nativeElement;
    if (!card) return;

    animate(card, {
      translateX: [
        { to: -10, duration: 50 },
        { to: 10, duration: 50 },
        { to: -8, duration: 50 },
        { to: 8, duration: 50 },
        { to: 0, duration: 50 },
      ],
      ease: 'inOutQuad',
    });
  }
}
