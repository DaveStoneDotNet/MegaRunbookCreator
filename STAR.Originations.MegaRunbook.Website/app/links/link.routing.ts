
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { LinkListComponent }   from './link-list.component'

const linksRoutes: Routes = [
    { path: 'links', component: LinkListComponent, data: { Title: 'Link List' } }
];

export const linkRouting: ModuleWithProviders = RouterModule.forChild(linksRoutes);

