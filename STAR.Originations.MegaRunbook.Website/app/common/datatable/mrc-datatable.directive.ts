
import { Directive }    from '@angular/core';
import { Input }        from '@angular/core';
import { Output }       from '@angular/core';
import { EventEmitter } from '@angular/core';
import { SimpleChange } from '@angular/core';
import { OnChanges }    from '@angular/core';
import { DoCheck }      from '@angular/core';

import { DataEvent }    from './i-data-event';
import { PageEvent }    from './i-page-event';
import { SortEvent }    from './i-sort-event';

@Directive({
    selector: 'table[mrc-data-input]',
    exportAs: 'mrcDataTable'                    // The name under which the component instance is exported in a template
})
export class DataTable implements OnChanges, DoCheck {

    @Input('mrc-data-input')    public inputData: any[] = [];
    @Input('mrc-rows-per-page') public rowsOnPage = 1000;
    @Input('mrc-current-page')  public activePage = 1;

    @Output() onDataRequested = new EventEmitter<PageEvent>();

    mrcDataInput: any[];

    onDataChange = new EventEmitter<DataEvent>();
    onSortChange = new EventEmitter<SortEvent>();
    onPageChange = new EventEmitter<PageEvent>();

    private sortBy: string | string[] = '';
    private sortOrder = 'asc';
    private mustRecalculateData = false;

    getSort(): SortEvent {
        return { sortBy: this.sortBy, sortOrder: this.sortOrder };
    }

    setSort(sortBy: string | string[], sortOrder: string): void {
        if (this.sortBy !== sortBy || this.sortOrder !== sortOrder) {
            this.sortBy = sortBy;
            this.sortOrder = sortOrder;
            this.mustRecalculateData = true;
            this.onSortChange.emit({ sortBy: sortBy, sortOrder: sortOrder });
        }
        this.onDataRequested.emit(this.getPage());
    }

    getPage(): PageEvent {
        return {
            activePage: this.activePage,
            rowsOnPage: this.rowsOnPage,
            dataLength: this.inputData.length
        };
    }

    setPage(activePage: number, rowsOnPage: number): void {
        if (this.rowsOnPage !== rowsOnPage || this.activePage !== activePage) {
            this.activePage = this.activePage !== activePage ? activePage : this.calculateNewActivePage(this.rowsOnPage, rowsOnPage);
            this.rowsOnPage = rowsOnPage;
            this.mustRecalculateData = true;
            this.onPageChange.emit({
                activePage: this.activePage,
                rowsOnPage: this.rowsOnPage,
                dataLength: this.inputData.length
            });
        }
    }

    ngOnChanges(changes: { [key: string]: SimpleChange }): any {
        if (changes['inputData']) {
            this.inputData = changes['inputData'].currentValue || [];
            this.recalculatePage();
            this.onPageChange.emit({
                activePage: this.activePage,
                rowsOnPage: this.rowsOnPage,
                dataLength: this.inputData.length
            });
            this.mustRecalculateData = true;
        }
    }

    ngDoCheck(): any {
        if (this.mustRecalculateData) {
            this.fillData();
            this.mustRecalculateData = false;
        }
    }

    private calculateNewActivePage(previousRowsOnPage: number, currentRowsOnPage: number): number {
        let firstRowOnPage = (this.activePage - 1) * previousRowsOnPage + 1;
        let newActivePage = Math.ceil(firstRowOnPage / currentRowsOnPage);
        return newActivePage;
    }

    private recalculatePage() {
        let lastPage = Math.ceil(this.inputData.length / this.rowsOnPage);
        this.activePage = lastPage < this.activePage ? lastPage : this.activePage;
        this.activePage = this.activePage || 1;
    }

    private fillData(): void {
        this.activePage = this.activePage;
        this.rowsOnPage = this.rowsOnPage;

        let offset = (this.activePage - 1) * this.rowsOnPage;
        let data = this.inputData;
        var sortBy = this.sortBy;
        if (typeof sortBy === 'string' || sortBy instanceof String) {
            //data = _.orderBy(data, this.caseInsensitiveIteratee(<string>sortBy), [this.sortOrder]);
        } else {
            //data = _.orderBy(data, sortBy, [this.sortOrder]);
        }
        //data = _.slice(data, offset, offset + this.rowsOnPage);
        this.mrcDataInput = data;
    }

    private caseInsensitiveIteratee(sortBy: string) {
        return (row: any): any => {
            var value = row[sortBy];
            if (value && typeof value === 'string' || value instanceof String) {
                return value.toLowerCase();
            }
            return value;
        };
    }
}