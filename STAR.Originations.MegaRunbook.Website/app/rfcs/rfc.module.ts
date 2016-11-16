
import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule }         from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { TypeaheadModule }     from 'ng2-bootstrap/components/typeahead';
import { TimepickerModule }    from 'ng2-bootstrap/components/timepicker';
import { DatepickerModule }    from 'ng2-bootstrap/components/datepicker';

import { MrcTimePickerModule } from '../common/timepicker/mrc-timepicker.module';

import { RfcService }          from './rfc.service';

import { MrcFocusDirective }   from '../common/mrc-focus.directive';

import { RfcListComponent }    from './rfc-list.component';
import { RfcAddComponent }     from './rfc-add.component';
import { RfcEditComponent }    from './rfc-edit.component';

import { rfcRouting }          from './rfc.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, ReactiveFormsModule, TypeaheadModule, TimepickerModule, DatepickerModule, rfcRouting, MrcTimePickerModule],
    declarations: [RfcListComponent, RfcAddComponent, RfcEditComponent, MrcFocusDirective],
    providers:    [RfcService]
})

export class RfcModule { }