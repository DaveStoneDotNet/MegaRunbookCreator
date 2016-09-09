
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { Input }          from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Params }         from '@angular/router';

import { TemplateService } from '../services/template.service';
import { RunbookTemplate } from '../entities/runbook-template.entity';

@Component({
    templateUrl: 'app/templates/template-detail.component.html',
    providers:   [TemplateService]
})

export class TemplateDetailComponent implements OnInit {

    runningSearch: Boolean;

    runbookTemplate: RunbookTemplate;

    constructor(private templateService: TemplateService, private route: ActivatedRoute) { }

    ngOnInit() {

        this.executeSearch();
    }

    private executeSearch(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            this.templateService.getRunbookTemplate(id)
                .subscribe(response => this.getRunbookTemplateOnSuccess(response), response => this.getRunbookTemplateOnError(response));
        });
    }

    private getRunbookTemplateOnSuccess(response: RunbookTemplate): void {

        this.runbookTemplate = response;

        this.runningSearch = false;
    }

    private getRunbookTemplateOnError(response): void {

        this.runningSearch = false;
   }
}