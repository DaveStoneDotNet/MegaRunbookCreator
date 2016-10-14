
import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule }         from '@angular/forms';

import { NG_TABLE_DIRECTIVES } from 'ng2-table';

import { PaginationModule }    from 'ng2-bootstrap/components/pagination';

import { NgTableComponent }    from './ng-table.component';

import { gridRouting }         from './grid.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, gridRouting, PaginationModule],
    declarations: [NgTableComponent, NG_TABLE_DIRECTIVES],
    providers:    []
})

export class GridModule { }