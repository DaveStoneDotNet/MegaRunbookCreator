
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

import { PageNotFoundComponent }    from './common/page-not-found.component'
import { NotAuthorizedComponent }   from './common/not-authorized.component'

import { AuthGuard }                from './services/auth-guard.service';
import { AuthService }              from './services/auth.service';

const mrcRoutes: Routes = [
    { path: '',                   component: HomeComponent }, 
    { path: 'home',               component: HomeComponent }, 
    { path: 'templatelist',       component: TemplateListComponent },
    { path: 'templatedetail/:id', component: TemplateDetailComponent }, 
    { path: 'templateform/:id',   component: RunbookStepFormComponent }, 
    { path: 'admin',              component: AdminComponent }, 
    { path: 'builds',             component: BuildsComponent }, 
    { path: 'runbooks',           component: RunbooksComponent }
];

const crisisCenterRoutes: Routes = [
    { path: 'crisis-center',      loadChildren: 'app/crisis-center/crisis-center.module#CrisisCenterModule' }
];

const commonRoutes: Routes = [
    { path: 'notauthorized',      component: NotAuthorizedComponent }, 
    { path: '**',                 component: PageNotFoundComponent }
];

const appRoutes: Routes = [
    ...mrcRoutes, 
    ...crisisCenterRoutes, 
    ...commonRoutes
];

export const appRoutingProviders: any[] = [AuthGuard, AuthService];

// Call the 'forRoot' method because we're providing a configured router at the root of the application. 
// The 'forRoot' method gives us the Router service providers and directives needed for routing.

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);

