
import { NgModule }         from '@angular/core';
import { CommonModule }     from '@angular/common';
import { FormsModule }      from '@angular/forms';

import { TooltipModule }    from 'ng2-bootstrap/components/tooltip';

import { PipeModule }       from '../pipes/pipe.module';

import { ReleaseComponent } from './release.component';

import { ReleaseService }   from './release.service';

import { releaseRouting }   from './release.routing';

@NgModule({
    imports: [CommonModule, FormsModule, releaseRouting, TooltipModule, PipeModule],
    declarations: [ReleaseComponent],
    providers: [ReleaseService]
})

export class ReleaseModule { }
