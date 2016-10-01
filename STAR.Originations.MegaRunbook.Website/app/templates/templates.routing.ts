
import { ModuleWithProviders }      from '@angular/core';
import { Routes }                   from '@angular/router';
import { RouterModule }             from '@angular/router';

import { RunbookStepFormComponent } from './runbook-step-form.component';
import { TemplateDetailComponent }  from './template-detail.component';
import { TemplateListComponent }    from './template-list.component';

const templatesRoutes: Routes = [
    { path: 'templatelist',        component: TemplateListComponent },
    { path: 'templatedetail/:id',  component: TemplateDetailComponent }, 
    { path: 'runbookStepForm/:id', component: RunbookStepFormComponent }, 
];

export const templatesRouting: ModuleWithProviders = RouterModule.forChild(templatesRoutes);

