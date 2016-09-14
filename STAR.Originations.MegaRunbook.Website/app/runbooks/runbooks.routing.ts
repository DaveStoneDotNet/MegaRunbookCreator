
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { RunbooksComponent }  from './runbooks.component';

const runbooksRoutes: Routes = [
    { path: 'runbooks', component: RunbooksComponent }
];

export const runbooksRouting: ModuleWithProviders = RouterModule.forChild(runbooksRoutes);

