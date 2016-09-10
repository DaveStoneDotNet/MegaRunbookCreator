
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { ActivatedRoute }  from '@angular/router';
import { Params }          from '@angular/router';
import { Router }          from '@angular/router';

import { TemplateService } from '../services/template.service';
import { RunbookTemplate } from '../entities/runbook-template.entity';
import { RunbookStep }     from '../entities/runbook-step.entity';

@Component({
    templateUrl: 'app/templates/template-detail.component.html',
    providers: [TemplateService]
})

export class TemplateDetailComponent implements OnInit {

    runningSearch: boolean;
    isNotCollapsed: boolean;

    runbookTemplate: RunbookTemplate;

    constructor(private templateService: TemplateService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {

        this.executeSearch();
    }

    // UI Events

    toggleCollapseAll() {
        this.isNotCollapsed = !this.isNotCollapsed;
        if (this.runbookTemplate && this.runbookTemplate.Steps) {
            this.runbookTemplate.Steps.forEach(s => s.IsNotCollapsed = this.isNotCollapsed);
        }
    }

    toggleCollapse(step: RunbookStep) {
        step.IsNotCollapsed = !step.IsNotCollapsed;
        this.syncCollapsables();
    }

    reloadTemplate() {
        this.executeSearch();
    }

    editRunbookStep(step: RunbookStep): void {
        let link = ['/templateform', step.ID];
        this.router.navigate(link);
    }

    // Data Access

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

    // Helper / Other

    private syncCollapsables() {

        // If *ALL* of the collapsibles are collapsed then sync the 'global' IsNotCollapsed to collapsed.
        // If *ALL* of the collapsibles are NOT collapsed then sync the 'global' IsNotCollapsed to not collapsed.

        let numerics: number[] = new Array(this.runbookTemplate.Steps.length);
        for (var i = 0; i < this.runbookTemplate.Steps.length; i++) {
            if (this.runbookTemplate.Steps[i].IsNotCollapsed) {
                numerics[i] = 1;
            } else {
                numerics[i] = 0;
            }
        }

        let is: number = 0;
        let isnot: number = 0;
        numerics.forEach(n => { if (n == 0) isnot = isnot + 1 });
        numerics.forEach(n => { if (n == 1) is = is + 1 });

        if (isnot == this.runbookTemplate.Steps.length) {
            this.isNotCollapsed = false;
        }
        if (is == this.runbookTemplate.Steps.length) {
            this.isNotCollapsed = true;
        }
    }
}