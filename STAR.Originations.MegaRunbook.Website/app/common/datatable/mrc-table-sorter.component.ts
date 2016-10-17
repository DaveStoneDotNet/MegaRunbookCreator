
import { Component }       from '@angular/core';
import { Input     }       from '@angular/core';

import { DataTable }       from './mrc-datatable.directive';
import { MrcSortSettings } from './mrc-sort-settings';
import { SortInfo }        from './i-sort-info';

@Component({
    selector:    'mrc-table-sorter',
    templateUrl: 'app/common/datatable/mrc-table-sorter.component.html', 
    styleUrls:  ['app/common/datatable/mrc-table-sorter.component.css']
})
export class MrcTableSorter {

    @Input('PropertyName') public PropertyName: string;

    private isAscending:  boolean = false;
    private isDescending: boolean = false;

    constructor(private mrcDataTable: DataTable) {
        mrcDataTable.onSortChange.subscribe((event: SortInfo) => {
            this.isAscending  = (event.PropertyName === this.PropertyName && event.SortOrder === MrcSortSettings.ASCENDING);
            this.isDescending = (event.PropertyName === this.PropertyName && event.SortOrder === MrcSortSettings.DESCENDING);
        })
    }

    private sort() {
        if (this.isDescending) {
            this.mrcDataTable.setSortInfo(MrcSortSettings.ASCENDING, this.PropertyName);
        } else {
            this.mrcDataTable.setSortInfo(MrcSortSettings.DESCENDING, this.PropertyName);
        }
    }
}
