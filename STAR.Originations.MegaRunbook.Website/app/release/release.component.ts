
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { ActivatedRoute }  from '@angular/router';
import { Params }          from '@angular/router';
import { Router }          from '@angular/router';

import { RunbookTemplate } from '../entities/runbook-template.entity';
import { RunbookStep }     from '../entities/runbook-step.entity';

import { ReleaseService }  from './release.service';

@Component({
    templateUrl: 'app/release/release.component.html',
    providers: [ReleaseService]
})

export class ReleaseComponent implements OnInit {

    Title = "Release";
    today: Date;

    runningSearch: boolean;
    isNotCollapsed: boolean;
    isReleaseSelectionCollapsed: boolean;
    isRfcCollapsed: boolean;

    runbookTemplate: RunbookTemplate;

    constructor(private releaseService: ReleaseService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {

        this.today = new Date();
        this.executeSearch();
    }

    // UI Events

    toggleReleaseSelection() {
        this.isReleaseSelectionCollapsed = !this.isReleaseSelectionCollapsed;
    }

    toggleRfcSection() {
        this.isRfcCollapsed = !this.isRfcCollapsed;
    }

    toggleRfcSteps() {
        this.isNotCollapsed = !this.isNotCollapsed;
        if (this.runbookTemplate && this.runbookTemplate.RunbookSteps) {
            this.runbookTemplate.RunbookSteps.forEach(s => s.IsNotCollapsed = this.isNotCollapsed);
        }
    }

    toggleCollapseAll() {
        this.isNotCollapsed = !this.isNotCollapsed;
        this.isRfcCollapsed = this.isNotCollapsed;
        if (this.runbookTemplate && this.runbookTemplate.RunbookSteps) {
            this.runbookTemplate.RunbookSteps.forEach(s => s.IsNotCollapsed = this.isNotCollapsed);
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
        let link = ['/runbookStepForm', step.ID];
        this.router.navigate(link);
    }

    // Data Access

    private executeSearch(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            this.releaseService.getRunbookTemplate(58)
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

        let numerics: number[] = new Array(this.runbookTemplate.RunbookSteps.length);
        for (var i = 0; i < this.runbookTemplate.RunbookSteps.length; i++) {
            if (this.runbookTemplate.RunbookSteps[i].IsNotCollapsed) {
                numerics[i] = 1;
            } else {
                numerics[i] = 0;
            }
        }

        let is: number = 0;
        let isnot: number = 0;
        numerics.forEach(n => { if (n == 0) isnot = isnot + 1 });
        numerics.forEach(n => { if (n == 1) is = is + 1 });

        if (isnot == this.runbookTemplate.RunbookSteps.length) {
            this.isNotCollapsed = false;
        }
        if (is == this.runbookTemplate.RunbookSteps.length) {
            this.isNotCollapsed = true;
        }
    }
}
