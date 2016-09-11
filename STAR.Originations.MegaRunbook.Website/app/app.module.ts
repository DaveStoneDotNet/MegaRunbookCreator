
import { NgModule }                 from '@angular/core';
import { BrowserModule }            from '@angular/platform-browser';
import { FormsModule }              from '@angular/forms';
import { HttpModule }               from '@angular/http';
                                 
import { LocationStrategy }         from '@angular/common';
import { HashLocationStrategy }     from '@angular/common';

import { routing }                  from './app.routing';
import { appRoutingProviders }      from './app.routing';

import { HeroesModule }             from './heroes/heroes.module';

import { AppComponent }             from './app.component';
import { HomeComponent }            from './home/home.component';
import { TemplateListComponent }    from './templates/template-list.component';
import { TemplateDetailComponent }  from './templates/template-detail.component'
import { RunbookStepFormComponent } from './templates/runbook-step-form.component'

import { AdminComponent }           from './admin/admin.component'
import { BuildsComponent }          from './builds/builds.component'
import { RunbooksComponent }        from './runbooks/runbooks.component'

import { CrisisCenterComponent }    from './heroes/crisis-center.component'

import { PageNotFoundComponent }    from './common/page-not-found.component'

import { HttpService }              from './services/http.service';
import { UserService }              from './services/user.service';
import { TemplateService }          from './services/template.service';
import { BlockUIService }           from './services/blockui.service';

@NgModule({
    imports:      [BrowserModule, FormsModule, HttpModule, routing, HeroesModule],
    declarations: [AppComponent, HomeComponent, TemplateListComponent, TemplateDetailComponent, RunbookStepFormComponent, AdminComponent, BuildsComponent, RunbooksComponent, CrisisCenterComponent, PageNotFoundComponent],
    providers:    [HttpService, UserService, TemplateService, BlockUIService, appRoutingProviders, { provide: LocationStrategy, useClass: HashLocationStrategy}], 
    bootstrap:    [AppComponent]
})
export class AppModule { }
