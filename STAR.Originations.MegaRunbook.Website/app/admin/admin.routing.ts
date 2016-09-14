
import { ModuleWithProviders }      from '@angular/core';
import { Routes }                   from '@angular/router';
import { RouterModule }             from '@angular/router';

import { AdminComponent } from './admin.component';

const adminRoutes: Routes = [
    { path: 'admin', component: AdminComponent }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(adminRoutes);

