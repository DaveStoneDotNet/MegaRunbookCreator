
import { Component } from '@angular/core';
import { Input     } from '@angular/core';

import { DataTable } from './mrc-datatable.directive';
import { SortEvent } from './i-sort-event';

@Component({
    selector:    'mrc-table-sorter',
    templateUrl: 'app/common/datatable/mrc-table-sorter.component.html', 
    styleUrls:  ['app/common/datatable/mrc-table-sorter.component.css']
})
export class MrcTableSorter {

    @Input('by') private sortBy: string;

    private isAscending: boolean = false;
    private isDescending: boolean = false;

    constructor(private mrcDataTable: DataTable) {
        mrcDataTable.onSortChange.subscribe((event: SortEvent) => {
            this.isAscending = (event.sortBy === this.sortBy && event.sortOrder === 'Ascending');
            this.isDescending = (event.sortBy === this.sortBy && event.sortOrder === 'Descending');
        })
    }

    private sort() {
        if (this.isAscending) {
            this.mrcDataTable.setSort(this.sortBy, 'Descending', this.sortBy);
        } else {
            this.mrcDataTable.setSort(this.sortBy, 'Ascending', this.sortBy);
        }
    }
}