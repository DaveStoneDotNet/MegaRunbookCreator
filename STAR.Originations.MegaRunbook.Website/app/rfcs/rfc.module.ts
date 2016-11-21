
import { NgModule }            from '@angular/core';
import { CommonModule }        from '@angular/common';
import { FormsModule }         from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { TypeaheadModule }     from 'ng2-bootstrap/components/typeahead';
import { TimepickerModule }    from 'ng2-bootstrap/components/timepicker';
import { DatepickerModule }    from 'ng2-bootstrap/components/datepicker';
import { PaginationModule }    from 'ng2-bootstrap/components/pagination';

import { ToastrModule }        from 'toastr-ng2';
import { provideToastr  }      from 'toastr-ng2';

import { PipeModule }          from '../pipes/pipe.module';

import { DataTableModule }     from '../common/datatable/mrc-datatable.module';

import { MrcTimePickerModule } from '../common/timepicker/mrc-timepicker.module';

import { RfcService }          from './rfc.service';

import { MrcFocusDirective }   from '../common/mrc-focus.directive';

import { RfcListComponent }    from './rfc-list.component';
import { RfcAddComponent }     from './rfc-add.component';
import { RfcEditComponent }    from './rfc-edit.component';

import { rfcRouting }          from './rfc.routing';

let toastrOptions = {
    positionClass: 'toast-bottom-right'
};

@NgModule({
    imports:      [CommonModule, FormsModule, ReactiveFormsModule, TypeaheadModule, TimepickerModule, DatepickerModule, ToastrModule, PaginationModule, rfcRouting, MrcTimePickerModule, DataTableModule, PipeModule],
    declarations: [RfcListComponent, RfcAddComponent, RfcEditComponent, MrcFocusDirective],
    providers:    [RfcService, provideToastr(toastrOptions)]
})

export class RfcModule { }