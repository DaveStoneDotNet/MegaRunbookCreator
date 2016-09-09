
import { ModuleWithProviders }     from '@angular/core';
import { Routes }                  from '@angular/router';
import { RouterModule }            from '@angular/router';

import { HomeComponent }           from './home/home.component'
import { TemplateListComponent }   from './templates/template-list.component'
import { TemplateDetailComponent } from './templates/template-detail.component'

const appRoutes: Routes = [
    { path: '',              component: HomeComponent }, 
    { path: 'home',          component: HomeComponent }, 
    { path: 'templates',     component: TemplateListComponent },
    { path: 'template/:id',  component: TemplateDetailComponent }
];

// call the 'forRoot' method because we're providing a configured router at the root of the application. 
// The 'forRoot' method gives us the Router service providers and directives needed for routing.

export const routing = RouterModule.forRoot(appRoutes);

