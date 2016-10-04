
import { NgModule }          from '@angular/core';
import { CommonModule }      from '@angular/common';
import { FormsModule }       from '@angular/forms';

import { ClipboardModule }   from 'angular2-clipboard';

import { LinkListComponent } from './link-list.component';

import { LinkService }       from './link.service';

import { linkRouting }       from './link.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, linkRouting, ClipboardModule],
    declarations: [LinkListComponent],
    providers:    [LinkService]
})

export class LinkModule { }