
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { NgTableComponent }    from './ng-table.component';
import { PrimeNgComponent }    from './prime-ng.component';

const gridRoutes: Routes = [
    { path: 'ngtable', component: NgTableComponent }, 
    { path: 'primeng', component: PrimeNgComponent }
];

export const gridRouting: ModuleWithProviders = RouterModule.forChild(gridRoutes);

