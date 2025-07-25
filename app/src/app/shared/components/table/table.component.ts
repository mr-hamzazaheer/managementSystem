import { Component, Input, Output, EventEmitter, effect, signal } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { TableService } from '../../service/table.service';
import { AngularSvgIconModule } from 'angular-svg-icon';

@Component({
  standalone: true,
  selector: 'app-table',
  imports: [CommonModule,AngularSvgIconModule],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  providers: [TableService],
})
export class TableComponent<T> {
  @Input() columns: Array<{ key: string; label: string; sortable?: boolean }> = [];
  @Input() fetchData!: (params: any) => Promise<{ data: T[]; total: number }>;

  data = signal<T[]>([]);
  total = signal(0);

  constructor(public tableService: TableService) {
    effect(() => {
      this.loadData();
    });
  }

  async loadData() {
    const { sort, page, size, filters, search } = this.tableService.tableParams();
    const response = await this.fetchData({ sort, page, size, filters, search });
    this.data.set(response.data);
    this.total.set(response.total);
  }

  onSearch(term: string) {
    this.tableService.updateSearch(term);
  }

  onFilterChange(name: string, value: string) {
    this.tableService.updateFilter(name, value);
  }

  onPageChange(page: number) {
    this.tableService.updatePage(page);
  }

  onSort(column: string) {
    this.tableService.updateSort(column);
  }

  onPageSizeChange(size: number) {
    this.tableService.updateSize(size);
  }
}
