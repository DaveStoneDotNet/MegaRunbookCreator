
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { NgTableComponent }    from './ng-table.component';

const gridRoutes: Routes = [
    { path: 'ngtable', component: NgTableComponent }
];

export const gridRouting: ModuleWithProviders = RouterModule.forChild(gridRoutes);

