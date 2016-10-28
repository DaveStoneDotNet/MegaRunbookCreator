
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { RfcListComponent }  from './rfc-list.component';

const rfcRoutes: Routes = [
    { path: 'rfcs', component: RfcListComponent }
];

export const rfcRouting: ModuleWithProviders = RouterModule.forChild(rfcRoutes);

