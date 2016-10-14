
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { DataTableDemo }       from './datatabledemo';

const primeRoutes: Routes = [
    { path: 'datatabledemo', component: DataTableDemo }
];

export const primeRouting: ModuleWithProviders = RouterModule.forChild(primeRoutes);

