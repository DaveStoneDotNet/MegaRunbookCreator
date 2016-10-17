
import { Component    } from '@angular/core';
import { Input        } from '@angular/core';
import { SimpleChange } from '@angular/core';
import { OnChanges    } from '@angular/core';
import { Optional     } from '@angular/core';

import { PageEvent    } from './i-page-event';
import { DataTable    } from './mrc-datatable.directive';

@Component({
    selector: 'mrcPaginator',
    template: `<ng-content></ng-content>`
})
export class Paginator implements OnChanges {

    @Input('mrcDataTable')
    private mrcDataTableInput: DataTable;
    private mrcDataTable:      DataTable;

    currentPage: number;
    recordsPerPage: number;
    totalRecordCount: number = 0;
    lastPage:   number;

    constructor(@Optional() private injectedMrcDataTable: DataTable) {
    }

    ngOnChanges(changes: { [key: string]: SimpleChange }): any {
        this.mrcDataTable = this.mrcDataTableInput || this.injectedMrcDataTable;
        this.onPageChangeSubscriber(this.mrcDataTable.getPage());
        this.mrcDataTable.onPageChange.subscribe(this.onPageChangeSubscriber);
    }

    setPage(pageNumber: number): void {
        if ((pageNumber > 0) && (pageNumber < this.lastPage+1)) {
            this.mrcDataTable.setPage(pageNumber, this.recordsPerPage);
        }
    }

    setRecordsPerPage(recordsPerPage: number): void {
        this.mrcDataTable.setPage(this.currentPage, recordsPerPage);
    }

    private onPageChangeSubscriber = (event: PageEvent) => {
        this.currentPage      = event.currentPage;
        this.totalRecordCount = event.totalRecordCount;
        this.recordsPerPage   = event.recordsPerPage;
        this.lastPage         = Math.ceil(this.totalRecordCount / this.recordsPerPage);
    };
}