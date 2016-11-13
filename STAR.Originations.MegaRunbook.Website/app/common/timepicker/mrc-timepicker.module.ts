
import { NgModule }                   from '@angular/core';
import { CommonModule }               from '@angular/common';
import { FormsModule }                from '@angular/forms';
import { ReactiveFormsModule }        from '@angular/forms';

import { TooltipModule }              from 'ng2-bootstrap/components/tooltip';

import { MrcDurationPickerComponent } from './mrc-durationpicker.component';
import { MrcTimePickerComponent }     from './mrc-timepicker.component';
import { MrcTimeInputComponent }      from './mrc-time-input.component';

@NgModule({
    imports:      [CommonModule, FormsModule, ReactiveFormsModule, TooltipModule],
    declarations: [MrcDurationPickerComponent, MrcTimePickerComponent, MrcTimeInputComponent],
    exports:      [MrcDurationPickerComponent, MrcTimePickerComponent, MrcTimeInputComponent]
})
export class MrcTimePickerModule {

}