
import { NgModule }       from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }    from '@angular/forms';

import { BuildsComponent } from './builds.component';

import { buildsRouting }   from './builds.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, buildsRouting],
    declarations: [BuildsComponent],
    providers:    []
})

export class BuildsModule { }