
import { Component } from '@angular/core';
import { Input }     from '@angular/core';
import { OnChanges } from '@angular/core';

import { DataTable } from './mrc-datatable.directive';
import { Paginator } from './paginator.component';

@Component({
    selector:    'mrcBootstrapPaginator',
    templateUrl: 'app/common/datatable/bootstrap-paginator.component.html' 
})
export class BootstrapPaginator implements OnChanges {

    @Input('rowsOnPageSet') private rowsOnPageSet = [];
    @Input('mrcDataTable')  private mrcDataTable: DataTable;

    private minRowsOnPage = 0;

    ngOnChanges(changes: any): any {

        if (changes.rowsOnPageSet) {
            //this.minRowsOnPage = _.min(this.rowsOnPageSet)
            this.minRowsOnPage = 5;
            var b = this.rowsOnPageSet;
        }
    }
}