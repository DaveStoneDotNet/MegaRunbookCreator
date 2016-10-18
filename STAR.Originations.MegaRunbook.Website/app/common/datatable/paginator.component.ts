
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
        if ((pageNumber > 0) && (pageNumber < this.lastPage + 1)) {
            this.mrcDataTable.setPage(pageNumber, this.recordsPerPage);
        }
    }

    setCurrentPage(pageNumber: number): void {
        this.setPage(this.currentPage + pageNumber);
    }

    setRecordsPerPage(recordsPerPage: number): void {
        this.mrcDataTable.setPage(1, recordsPerPage);
    }

    private onPageChangeSubscriber = (event: PageEvent) => {
        this.currentPage      = event.currentPage;
        this.totalRecordCount = event.totalRecordCount;
        this.recordsPerPage   = event.recordsPerPage;
        this.lastPage         = Math.ceil(this.totalRecordCount / this.recordsPerPage);
    };

    isWorking(): boolean {
        if (this.mrcDataTable) {
            return this.mrcDataTable.isWorking;
        } else {
            return false;
        }
    }

    // ---

    isFirstPage(): boolean {
        return true;
    }

    isLastPage(): boolean {
        return true;
    }

    // ---

    isFirstBeforeCurrent(): boolean {
        return this.currentPage > 1;
    }
    isSecondBeforeCurrent(): boolean {
        return this.currentPage > 2;
    }
    isThirdBeforeCurrent(): boolean {
        return this.currentPage > 3 && this.currentPage + 2 > this.lastPage;
    }
    isFourthBeforeCurrent(): boolean {
        return this.currentPage > 4 && this.currentPage + 1 > this.lastPage;
    }

    isFirstAfterCurrent(): boolean {
        return this.currentPage + 1 <= this.lastPage;
    }
    isSecondAfterCurrent(): boolean {
        return this.currentPage + 2 <= this.lastPage;
    }
    isThirdAfterCurrent(): boolean {
        return this.currentPage + 3 <= this.lastPage && this.currentPage < 3;
    }
    isFourthAfterCurrent(): boolean {
        return this.currentPage + 4 <= this.lastPage && this.currentPage < 2;
    }

    // ---

    isFirstDisabled(): boolean {
        return this.currentPage == 1;
    }
    isPreviousDisabled(): boolean {
        return this.currentPage == 1;
    }
    isNextDisabled(): boolean {
        return this.currentPage == this.lastPage;
    }
    isLastDisabled(): boolean {
        return this.currentPage >= this.lastPage;
    }

}