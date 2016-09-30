
import { NgModule }                 from '@angular/core';
import { BrowserModule }            from '@angular/platform-browser';
import { FormsModule }              from '@angular/forms';
import { ReactiveFormsModule }      from '@angular/forms';
import { HttpModule }               from '@angular/http';
                                 
import { LocationStrategy }         from '@angular/common';
import { HashLocationStrategy }     from '@angular/common';

import { routing }                  from './app.routing';
import { appRoutingProviders }      from './app.routing';

import { HeroesModule }             from './heroes/heroes.module';
import { AdminModule }              from './admin/admin.module';
import { BuildsModule }             from './builds/builds.module';
import { RunbooksModule }           from './runbooks/runbooks.module';
import { TemplatesModule }          from './templates/templates.module';

import { AppComponent }             from './app.component';
import { HomeComponent }            from './home/home.component';

import { PageNotFoundComponent }    from './common/page-not-found.component'
import { NotAuthorizedComponent }   from './common/not-authorized.component'

import { HttpService }              from './services/http.service';
import { UserService }              from './services/user.service';
import { TemplateService }          from './services/template.service';
import { BlockUIService }           from './services/blockui.service';
import { DialogService }            from './services/dialog.service';
import { CanDeactivateGuard }       from './services/can-deactivate-guard.service';
import { AuthGuard }                from './services/auth-guard.service';
import { AuthService }              from './services/auth.service';

import { PluralPipe }               from './pipes/plural.pipe';

@NgModule({
    imports:      [BrowserModule, FormsModule, ReactiveFormsModule, HttpModule, routing, HeroesModule, AdminModule, BuildsModule, RunbooksModule, TemplatesModule],
    declarations: [AppComponent, HomeComponent, PageNotFoundComponent, NotAuthorizedComponent],
    providers:    [HttpService, UserService, TemplateService, BlockUIService, DialogService, CanDeactivateGuard, AuthGuard, AuthService, appRoutingProviders, { provide: LocationStrategy, useClass: HashLocationStrategy}], 
    bootstrap:    [AppComponent]
})
export class AppModule { }
