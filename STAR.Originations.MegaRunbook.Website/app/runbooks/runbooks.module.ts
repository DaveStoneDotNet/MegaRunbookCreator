
import { NgModule }          from '@angular/core';
import { CommonModule }      from '@angular/common';
import { FormsModule }       from '@angular/forms';

import { RunbooksComponent } from './runbooks.component';

import { runbooksRouting }   from './runbooks.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, runbooksRouting],
    declarations: [RunbooksComponent],
    providers:    []
})

export class RunbooksModule { }