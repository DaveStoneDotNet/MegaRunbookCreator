
import { Component } from '@angular/core';
import { Input     } from '@angular/core';

import { DataTable } from './mrc-datatable.directive';
import { SortEvent } from './i-sort-event';

@Component({
    selector:    'mfDefaultSorter',
    templateUrl: 'app/common/datatable/default-sorter.component.html'
})
export class DefaultSorter {

    @Input('by') private sortBy: string;

    private isSortedByMeAsc: boolean = false;
    private isSortedByMeDesc: boolean = false;

    constructor(private mrcDataTable: DataTable) {
        mrcDataTable.onSortChange.subscribe((event: SortEvent) => {
            this.isSortedByMeAsc = (event.sortBy === this.sortBy && event.sortOrder === 'asc');
            this.isSortedByMeDesc = (event.sortBy === this.sortBy && event.sortOrder === 'desc');
        })
    }

    private sort() {
        if (this.isSortedByMeAsc) {
            this.mrcDataTable.setSort(this.sortBy, 'desc');
        } else {
            this.mrcDataTable.setSort(this.sortBy, 'asc');
        }
    }
}