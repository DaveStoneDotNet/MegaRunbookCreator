
import { NgModule }          from '@angular/core';
import { CommonModule }      from '@angular/common';
import { FormsModule }       from '@angular/forms';

import { ClipboardModule }   from 'angular2-clipboard';
import { ToastrModule }      from 'toastr-ng2';
import { provideToastr  }    from 'toastr-ng2';

import { LinkListComponent } from './link-list.component';

import { LinkService }       from './link.service';

import { linkRouting }       from './link.routing';

let toastrOptions = {
    positionClass: 'toast-bottom-right'
};

@NgModule({
    imports:      [CommonModule, FormsModule, linkRouting, ClipboardModule, ToastrModule],
    declarations: [LinkListComponent],
    providers:    [LinkService, provideToastr(toastrOptions)]
})

export class LinkModule { }