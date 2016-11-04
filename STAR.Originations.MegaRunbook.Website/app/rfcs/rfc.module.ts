
import { NgModule }                   from '@angular/core';
import { CommonModule }               from '@angular/common';
import { FormsModule }                from '@angular/forms';
import { ReactiveFormsModule }        from '@angular/forms';

import { TypeaheadModule }            from 'ng2-bootstrap/components/typeahead';
import { TimepickerModule }           from 'ng2-bootstrap/components/timepicker';

import { RfcListComponent }           from './rfc-list.component';
import { RfcAddComponent }            from './rfc-add.component';
import { RfcEditComponent }           from './rfc-edit.component';

import { MrcTimePickerComponent }     from '../common/timepicker/mrc-timepicker.component'
import { MrcDurationPickerComponent } from '../common/timepicker/mrc-durationpicker.component'
import { MrcTimeInputComponent }      from '../common/timepicker/mrc-time-input.component'

import { rfcRouting }                 from './rfc.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, ReactiveFormsModule, TypeaheadModule, TimepickerModule, rfcRouting],
    declarations: [RfcListComponent, RfcAddComponent, RfcEditComponent, MrcTimePickerComponent, MrcDurationPickerComponent, MrcTimeInputComponent],
    providers:    []
})

export class RfcModule { }