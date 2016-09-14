
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { BuildsComponent }     from './builds.component';

const buildsRoutes: Routes = [
    { path: 'builds', component: BuildsComponent }
];

export const buildsRouting: ModuleWithProviders = RouterModule.forChild(buildsRoutes);

