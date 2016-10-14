
import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }    from '@angular/forms';

import { BuildsComponent } from './builds.component';

import { buildsRouting }   from './builds.routing';

import { DataTableModule } from '../common/datatable/mrc-datatable.module';

@NgModule({
    imports:      [CommonModule, FormsModule, buildsRouting, DataTableModule],
    declarations: [BuildsComponent],
    providers:    []
})

export class BuildsModule { }