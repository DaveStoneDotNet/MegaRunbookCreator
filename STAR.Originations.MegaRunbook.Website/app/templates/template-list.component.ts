
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { Router }               from '@angular/router';

import { MessageService }       from '../services/message.service';

import { RunbookTemplate }      from '../entities/runbook-template.entity';
import { PagedRunbookTemplate } from '../entities/paged-runbook-template.entity';

import { TemplateService }      from './template.service';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/templates/template-list.component.html',
    styleUrls:   ['app/templates/template-list.component.css'],
    providers:   [TemplateService]
})

export class TemplateListComponent implements OnInit, OnDestroy {

    runningSearch: boolean;

    runbookTemplates: RunbookTemplate[];
    searchResults: RunbookTemplate[];
    delaySearch: boolean;
    searchTemplateName: string;

    private subscription: Subscription;

    constructor(private templateService: TemplateService, private messageService: MessageService, private router: Router) { }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
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

    clearSearchTemplateClicked(): void {
        this.messageService.sendTextMessage('Clearing...');
        this.searchTemplateName = '';
        this.searchTemplateNameChanged('');
    }

    private executeSearch(): void {

        this.messageService.sendTextMessage('Searching...');

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.subscription = this.templateService.getRunbookTemplates()
                .subscribe(
                response => this.getTemplatesOnSuccess(response),
                response => this.getTemplatesOnError(response)
            );
        },
            miliseconds);
    }

    private getTemplatesOnSuccess(response: PagedRunbookTemplate): void {

        this.runbookTemplates = response.Items;
        this.searchResults = response.Items;

        this.runningSearch = false;
        this.messageService.sendTextMessage('Ready');
    }

    private getTemplatesOnError(response): void {

        this.runningSearch = false;
    }
}