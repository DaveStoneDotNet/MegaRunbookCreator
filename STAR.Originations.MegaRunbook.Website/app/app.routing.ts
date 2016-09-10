﻿
import { ModuleWithProviders }      from '@angular/core';
import { Routes }                   from '@angular/router';
import { RouterModule }             from '@angular/router';

import { HomeComponent }            from './home/home.component'
import { TemplateListComponent }    from './templates/template-list.component'
import { TemplateDetailComponent }  from './templates/template-detail.component'
import { RunbookStepFormComponent } from './templates/runbook-step-form.component'

import { AdminComponent }           from './admin/admin.component'
import { BuildsComponent }          from './builds/builds.component'
import { RunbooksComponent }        from './runbooks/runbooks.component'

import { CrisisCenterComponent }    from './heroes/crisis-center.component'
import { HeroListComponent }        from './heroes/hero-list.component'
import { HeroDetailComponent }      from './heroes/hero-detail.component'
import { PageNotFoundComponent }    from './heroes/page-not-found.component'

const appRoutes: Routes = [
    { path: '',                   component: HomeComponent }, 
    { path: 'home',               component: HomeComponent }, 
    { path: 'templatelist',       component: TemplateListComponent },
    { path: 'templatedetail/:id', component: TemplateDetailComponent }, 
    { path: 'templateform/:id',   component: RunbookStepFormComponent }, 
    { path: 'admin',              component: AdminComponent }, 
    { path: 'builds',             component: BuildsComponent }, 
    { path: 'runbooks',           component: RunbooksComponent }, 

    { path: 'crisis-center',      component: CrisisCenterComponent }, 
    { path: 'heroes',             component: HeroListComponent,            data: { Title: 'Hero List' } }, 
    { path: 'hero/:id',           component: HeroDetailComponent }, 
    { path: '**',                 component: PageNotFoundComponent }
];

export const appRoutingProviders: any[] = [];

// Call the 'forRoot' method because we're providing a configured router at the root of the application. 
// The 'forRoot' method gives us the Router service providers and directives needed for routing.

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);

