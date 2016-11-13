
import { NgModule }                   from '@angular/core';
import { CommonModule }               from '@angular/common';
import { FormsModule }                from '@angular/forms';
import { ReactiveFormsModule }        from '@angular/forms';

import { TypeaheadModule }            from 'ng2-bootstrap/components/typeahead';
import { TimepickerModule }           from 'ng2-bootstrap/components/timepicker';

import { MrcTimePickerModule }        from '../common/timepicker/mrc-timepicker.module';

import { RfcListComponent }           from './rfc-list.component';
import { RfcAddComponent }            from './rfc-add.component';
import { RfcEditComponent }           from './rfc-edit.component';

import { rfcRouting }                 from './rfc.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, ReactiveFormsModule, TypeaheadModule, TimepickerModule, rfcRouting, MrcTimePickerModule],
    declarations: [RfcListComponent, RfcAddComponent, RfcEditComponent],
    providers:    []
})

export class RfcModule { }