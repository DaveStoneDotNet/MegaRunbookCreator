
import { NgModule }          from '@angular/core';
import { CommonModule }      from '@angular/common';
import { FormsModule }       from '@angular/forms';

import { LinkListComponent } from './link-list.component';

import { LinkService }       from './link.service';

import { linkRouting }       from './link.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, linkRouting],
    declarations: [LinkListComponent],
    providers:    [LinkService]
})

export class LinkModule { }