
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

import { TemplateService } from '../services/template.service';
import { RunbookTemplate } from '../entities/runbook-template.entity';

@Component({
    templateUrl: 'app/templates/template-list.component.html',
    styleUrls: ['app/templates/template-list.component.css'],
    providers: [TemplateService]
})

export class TemplateListComponent implements OnInit {

    runningSearch: boolean;

    runbookTemplates: RunbookTemplate[];
    searchResults: RunbookTemplate[];
    delaySearch: boolean;
    searchTemplateName: string;

    constructor(private templateService: TemplateService, private router: Router) { }

    ngOnInit() {
        this.executeSearch();
    }

    searchTemplateNameChanged(newValue): void {

        this.delaySearch = true;

        if (newValue !== '') {
            this.searchTemplateName = newValue;
            this.searchResults = this.runbookTemplates.filter(item => item.Name.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
    }

    runbookTemplateClicked(selectedRunbookTemplate: RunbookTemplate): void {
        let link = ['/templatedetail', selectedRunbookTemplate.ID];
        this.router.navigate(link);
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.templateService.searchTemplates()
                .subscribe(
                response => this.getTemplatesOnSuccess(response),
                response => this.getTemplatesOnError(response));

        },
            miliseconds);
    }

    private getTemplatesOnSuccess(response: RunbookTemplate[]): void {

        this.runbookTemplates = response;
        this.searchResults = response;

        this.runningSearch = false;
    }

    private getTemplatesOnError(response): void {

        this.runningSearch = false;
    }
}