
import { NgModule           } from '@angular/core';
import { CommonModule       } from '@angular/common';
import { TooltipModule      } from 'ng2-bootstrap/components/tooltip';

import { DataTable          } from './mrc-datatable.directive';
import { MrcTableSorter     } from './mrc-table-sorter.component';
import { Paginator          } from './paginator.component';
import { BootstrapPaginator } from './bootstrap-paginator.component';

@NgModule({
    imports:      [CommonModule, TooltipModule],
    declarations: [DataTable, MrcTableSorter, Paginator, BootstrapPaginator ],
    exports:      [DataTable, MrcTableSorter, Paginator, BootstrapPaginator ]
})
export class DataTableModule {

}