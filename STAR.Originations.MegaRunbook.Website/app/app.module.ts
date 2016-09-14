
import { NgModule }                 from '@angular/core';
import { BrowserModule }            from '@angular/platform-browser';
import { FormsModule }              from '@angular/forms';
import { HttpModule }               from '@angular/http';
                                 
import { LocationStrategy }         from '@angular/common';
import { HashLocationStrategy }     from '@angular/common';

import { routing }                  from './app.routing';
import { appRoutingProviders }      from './app.routing';

import { HeroesModule }             from './heroes/heroes.module';
import { TemplatesModule }          from './templates/templates.module';

import { AppComponent }             from './app.component';
import { HomeComponent }            from './home/home.component';

import { AdminComponent }           from './admin/admin.component'
import { BuildsComponent }          from './builds/builds.component'
import { RunbooksComponent }        from './runbooks/runbooks.component'

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

@NgModule({
    imports:      [BrowserModule, FormsModule, HttpModule, routing, HeroesModule, TemplatesModule],
    declarations: [AppComponent, HomeComponent, AdminComponent, BuildsComponent, RunbooksComponent, PageNotFoundComponent, NotAuthorizedComponent],
    providers:    [HttpService, UserService, TemplateService, BlockUIService, DialogService, CanDeactivateGuard, AuthGuard, AuthService, appRoutingProviders, { provide: LocationStrategy, useClass: HashLocationStrategy}], 
    bootstrap:    [AppComponent]
})
export class AppModule { }
