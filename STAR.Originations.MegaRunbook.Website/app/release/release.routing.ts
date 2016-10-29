
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { ReleaseComponent }  from './release.component';

const releaseRoutes: Routes = [
    { path: 'release', component: ReleaseComponent }
];

export const releaseRouting: ModuleWithProviders = RouterModule.forChild(releaseRoutes);
