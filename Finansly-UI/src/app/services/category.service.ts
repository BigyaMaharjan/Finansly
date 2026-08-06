import { Injectable, inject } from '@angular/core';
import {
  Client,
  CreateCategoryDto,
  UpdateCategoryDto,
  CreateCategoryWithTransactionsDto,
  CategoryDto,
  CategoryTypeLookupDto,
  CategoryWithTransactionsResultDto,
} from '../api-client';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private client = inject(Client);

  getTypes(): Promise<CategoryTypeLookupDto[]> {
    return new Promise((resolve, reject) => {
      this.client.types().subscribe({
        next: (res) => resolve(res.data ?? []),
        error: (err) => reject(err),
      });
    });
  }

  getAll(): Promise<CategoryDto[]> {
    return new Promise((resolve, reject) => {
      this.client.categoriesGET().subscribe({
        next: (res) => resolve(res.data ?? []),
        error: (err) => reject(err),
      });
    });
  }

  getById(id: string): Promise<CategoryDto> {
    return new Promise((resolve, reject) => {
      this.client.categoriesGET2(id).subscribe({
        next: (res) => {
          if (res.data) resolve(res.data);
          else reject(new Error('Category not found'));
        },
        error: (err) => reject(err),
      });
    });
  }

  create(dto: CreateCategoryDto): Promise<string> {
    return new Promise((resolve, reject) => {
      this.client.categoriesPOST(dto).subscribe({
        next: (res) => {
          if (res.data) resolve(res.data);
          else reject(new Error('Failed to create category'));
        },
        error: (err) => reject(err),
      });
    });
  }

  update(id: string, dto: UpdateCategoryDto): Promise<string> {
    return new Promise((resolve, reject) => {
      this.client.categoriesPUT(id, dto).subscribe({
        next: (res) => {
          if (res.data) resolve(res.data);
          else reject(new Error('Failed to update category'));
        },
        error: (err) => reject(err),
      });
    });
  }

  delete(id: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.client.categoriesDELETE(id).subscribe({
        next: (res) => resolve(res.data ?? false),
        error: (err) => reject(err),
      });
    });
  }

  createWithTransactions(
    dto: CreateCategoryWithTransactionsDto,
  ): Promise<CategoryWithTransactionsResultDto> {
    return new Promise((resolve, reject) => {
      this.client.withTransactions(dto).subscribe({
        next: (res) => {
          if (res.data) resolve(res.data);
          else reject(new Error('Failed to create category with transactions'));
        },
        error: (err) => reject(err),
      });
    });
  }
}
