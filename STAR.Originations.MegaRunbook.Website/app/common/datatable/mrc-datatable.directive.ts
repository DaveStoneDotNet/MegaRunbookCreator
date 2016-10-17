
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

import { PagedData }    from './i-paged-data';

@Directive({
    selector: 'table[mrc-data-input]',
    exportAs: 'mrcDataTable'                    // The name under which the component instance is exported in a template
})
export class DataTable implements OnChanges, DoCheck {

    @Input('mrc-data-input')       public inputData: PagedData;
    @Input('mrc-records-per-page') public recordsPerPage = 10;
    @Input('mrc-current-page')     public currentPage = 1;

    @Output() onDataRequested = new EventEmitter<DataEvent>();
    @Output() onDataReceived  = new EventEmitter<DataEvent>();

    mrcDataInput: PagedData;

    onDataChange = new EventEmitter<DataEvent>();
    onSortChange = new EventEmitter<SortEvent>();
    onPageChange = new EventEmitter<PageEvent>();

    private PropertyName: string;
    private SortOrder = 'Ascending';
    private mustRecalculateData = false;

    getDataEvent(): DataEvent {
        return {
                    TotalRecordCount: this.inputData.TotalRecordCount,
                    PageNumber: this.currentPage,
                    RecordsPerPage: this.recordsPerPage, 
                    SortInfo: this.getSort()
                };
    }

    getSort(): SortEvent {
        if (this.PropertyName) {
            return { SortOrder: this.SortOrder, PropertyName: this.PropertyName };
        } else {
            return null;
        }
    }

    setSort(sortOrder: string, propertyName: string): void {
        if (this.PropertyName !== propertyName || this.SortOrder !== sortOrder) {
            this.SortOrder = sortOrder;
            this.PropertyName = propertyName;
            this.mustRecalculateData = true;
            this.onSortChange.emit({ SortOrder: sortOrder, PropertyName: propertyName });
        }
        this.onDataRequested.emit(this.getDataEvent());
    }

    getPage(): PageEvent {
        return {
            currentPage: this.currentPage,
            recordsPerPage: this.recordsPerPage,
            totalRecordCount: this.inputData.TotalRecordCount
        };
    }

    setPage(currentPage: number, recordsPerPage: number): void {
        if (this.recordsPerPage !== recordsPerPage || this.currentPage !== currentPage) {
            this.currentPage = this.currentPage = currentPage;
            this.recordsPerPage = recordsPerPage;
            this.mustRecalculateData = true;
            this.onPageChange.emit({
                currentPage: this.currentPage,
                recordsPerPage: this.recordsPerPage,
                totalRecordCount: this.inputData.TotalRecordCount
            });
            this.onDataRequested.emit(this.getDataEvent());
        }
    }

    ngOnChanges(changes: { [key: string]: SimpleChange }): any {
        if (changes['inputData']) {
            this.inputData = changes['inputData'].currentValue || { Items: [] };
            this.onPageChange.emit({
                currentPage: this.inputData.PageNumber,
                recordsPerPage: this.inputData.RecordsPerPage,
                totalRecordCount: this.inputData.TotalRecordCount
            });
            this.mustRecalculateData = true;
        }
    }

    ngDoCheck(): any {
        this.mrcDataInput = this.inputData;
    }
}