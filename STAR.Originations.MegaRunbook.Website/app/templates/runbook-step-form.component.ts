
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { ActivatedRoute }  from '@angular/router';
import { Params }          from '@angular/router';

import { RunbookStep }     from '../entities/runbook-step.entity';

import { TemplateService } from './template.service';

@Component({
    templateUrl: 'app/templates/runbook-step-form.component.html',
    providers:   [TemplateService]
})

export class RunbookStepFormComponent implements OnInit {

    active = true;
    submitted = false;

    runningSearch: boolean;

    runbookStep: RunbookStep;

    statuses = ['Started', 'Delayed', 'Finished'];

    constructor(private templateService: TemplateService, private route: ActivatedRoute) { }

    ngOnInit() {

        this.executeSearch();
    }

    // UI Events

    // Data Access

    private executeSearch(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            this.templateService.getRunbookStep(id)
                .subscribe(response => this.getRunbookStepOnSuccess(response), response => this.getRunbookStepOnError(response));
        });
    }

    private getRunbookStepOnSuccess(response: RunbookStep): void {

        this.runbookStep = response;

        this.runningSearch = false;
    }

    private getRunbookStepOnError(response): void {

        this.runningSearch = false;
    }

    // Helper / Other

}