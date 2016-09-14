﻿
import { NgModule }                 from '@angular/core';
import { CommonModule }             from '@angular/common';
import { FormsModule }              from '@angular/forms';

import { RunbookStepFormComponent } from './runbook-step-form.component';
import { TemplateDetailComponent }  from './template-detail.component';
import { TemplateListComponent }    from './template-list.component';

import { TemplateService }          from '../services/template.service';

import { templatesRouting }         from './templates.routing';

@NgModule({
    imports:      [CommonModule, FormsModule, templatesRouting],
    declarations: [RunbookStepFormComponent, TemplateDetailComponent, TemplateListComponent],
    providers:    [TemplateService]
})

export class TemplatesModule { }