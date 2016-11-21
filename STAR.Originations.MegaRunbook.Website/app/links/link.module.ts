
import { NgModule }          from '@angular/core';
import { CommonModule }      from '@angular/common';
import { FormsModule }       from '@angular/forms';

import { ClipboardModule }   from 'angular2-clipboard';
import { ToastrModule }      from 'toastr-ng2';
import { provideToastr  }    from 'toastr-ng2';

import { PaginationModule }  from 'ng2-bootstrap/components/pagination';
import { DropdownModule }    from 'ng2-bootstrap/components/dropdown';

import { PipeModule }        from '../pipes/pipe.module';

import { DataTableModule }   from '../common/datatable/mrc-datatable.module';

import { LinkListComponent } from './link-list.component';

import { LinkService }       from './link.service';

import { linkRouting }       from './link.routing';

let toastrOptions = {
    positionClass: 'toast-bottom-right'
};

@NgModule({
    imports:      [CommonModule, FormsModule, linkRouting, ClipboardModule, ToastrModule, PaginationModule, DropdownModule, DataTableModule, PipeModule],
    declarations: [LinkListComponent],
    providers:    [LinkService, provideToastr(toastrOptions)]
})

export class LinkModule { }