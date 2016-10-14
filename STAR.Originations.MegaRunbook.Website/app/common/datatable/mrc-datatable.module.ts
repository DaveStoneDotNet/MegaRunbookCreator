
import { NgModule           } from '@angular/core';
import { CommonModule       } from '@angular/common';

import { DataTable          } from './mrc-datatable.directive';
import { DefaultSorter      } from './default-sorter.component';
import { Paginator          } from './paginator.component';
import { BootstrapPaginator } from './bootstrap-paginator.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        DataTable,
        DefaultSorter,
        Paginator,
        BootstrapPaginator
    ],
    exports: [
        DataTable,
        DefaultSorter,
        Paginator,
        BootstrapPaginator
    ]
})
export class DataTableModule {

}