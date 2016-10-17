
import { Component }     from '@angular/core';
import { Input     }     from '@angular/core';

import { DataTable }     from './mrc-datatable.directive';
import { SortEvent }     from './i-sort-event';

@Component({
    selector:    'mrc-table-sorter',
    templateUrl: 'app/common/datatable/mrc-table-sorter.component.html', 
    styleUrls:  ['app/common/datatable/mrc-table-sorter.component.css']
})
export class MrcTableSorter {

    @Input('PropertyName') private PropertyName: string;

    private isAscending: boolean = false;
    private isDescending: boolean = false;

    constructor(private mrcDataTable: DataTable) {
        mrcDataTable.onSortChange.subscribe((event: SortEvent) => {
            this.isAscending = (event.PropertyName === this.PropertyName && event.SortOrder === 'Ascending');
            this.isDescending = (event.PropertyName === this.PropertyName && event.SortOrder === 'Descending');
        })
    }

    private sort() {
        if (this.isAscending) {
            this.mrcDataTable.setSort('Descending', this.PropertyName);
        } else {
            this.mrcDataTable.setSort('Ascending', this.PropertyName);
        }
    }
}