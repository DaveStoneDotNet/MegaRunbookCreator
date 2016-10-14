
import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule }         from '@angular/forms';

import { InputTextModule }     from 'primeng/primeng';
import { DataTableModule }     from 'primeng/primeng';
import { SharedModule }        from 'primeng/primeng';

import { CarService }          from './carservice';

import { DataTableDemo  }      from './datatabledemo';

import { primeRouting }        from './prime.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, primeRouting, SharedModule, InputTextModule, DataTableModule],
    declarations: [DataTableDemo],
    providers:    [CarService]
})

export class PrimeModule { }