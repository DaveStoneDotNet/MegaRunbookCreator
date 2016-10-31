
import { NgModule }         from '@angular/core';
import { CommonModule }     from '@angular/common';
import { FormsModule }      from '@angular/forms';

import { RfcListComponent } from './rfc-list.component';
import { RfcAddComponent }  from './rfc-add.component';
import { RfcEditComponent } from './rfc-edit.component';

import { rfcRouting }       from './rfc.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, rfcRouting],
    declarations: [RfcListComponent, RfcAddComponent, RfcEditComponent],
    providers:    []
})

export class RfcModule { }