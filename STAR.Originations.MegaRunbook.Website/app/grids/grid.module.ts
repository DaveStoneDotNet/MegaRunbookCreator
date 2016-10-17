
import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule }         from '@angular/forms';

import { NG_TABLE_DIRECTIVES } from 'ng2-table';

import { PaginationModule }    from 'ng2-bootstrap/components/pagination';

import { DataTableModule } from '../common/datatable/mrc-datatable.module';

import { NgTableComponent }    from './ng-table.component';
import { MrcTableComponent }   from './mrc-table.component';

import { gridRouting }         from './grid.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, gridRouting, PaginationModule, DataTableModule],
    declarations: [NgTableComponent, NG_TABLE_DIRECTIVES, MrcTableComponent],
    providers:    []
})

export class GridModule { }