
import { NgModule }                from '@angular/core';
import { BrowserModule }           from '@angular/platform-browser';
import { FormsModule }             from '@angular/forms';
import { HttpModule }              from '@angular/http';
                                 
import { LocationStrategy }        from '@angular/common';
import { HashLocationStrategy }    from '@angular/common';

import { routing }                 from './app.routing';
import { AppComponent }            from './app.component';
import { HomeComponent }           from './home/home.component';
import { TemplateListComponent }   from './templates/template-list.component';
import { TemplateDetailComponent } from './templates/template-detail.component'

import { HttpService }             from './services/http.service';
import { UserService }             from './services/user.service';
import { TemplateService }         from './services/template.service';
import { BlockUIService }          from './services/blockui.service';

@NgModule({
    imports:      [BrowserModule, FormsModule, HttpModule, routing],
    declarations: [AppComponent, HomeComponent, TemplateListComponent, TemplateDetailComponent],
    providers:    [HttpService, UserService, TemplateService, BlockUIService, { provide: LocationStrategy, useClass: HashLocationStrategy}], 
    bootstrap:    [AppComponent]
})
export class AppModule { }
