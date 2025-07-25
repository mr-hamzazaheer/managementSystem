import { Injectable, computed, signal } from '@angular/core';

@Injectable()
export class TableService {
  private _search = signal('');
  private _sort = signal<string | null>('');
  private _page = signal(1);
  private _size = signal(10);
  private _filters = signal<{ [key: string]: string }>({});

  updateSearch(term: string) {
    this._search.set(term);
    this._page.set(1);
  }

  updateSort(column: string) {
    const current = this._sort();
    this._sort.set(current === column ? `-${column}` : column);
    this._page.set(1);
  }

  updateFilter(name: string, value: string) {
    const filters = { ...this._filters(), [name]: value };
    this._filters.set(filters);
    this._page.set(1);
  }

  updatePage(page: number) {
    this._page.set(page);
  }

  updateSize(size: number) {
    this._size.set(size);
    this._page.set(1);
  }

  tableParams = computed(() => ({
    search: this._search(),
    sort: this._sort(),
    page: this._page(),
    size: this._size(),
    filters: this._filters()
  }));
}
