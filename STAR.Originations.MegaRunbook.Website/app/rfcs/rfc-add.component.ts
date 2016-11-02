
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { Router }               from '@angular/router';

import { MessageService }       from '../services/message.service';

import { RunbookTemplate }      from '../entities/runbook-template.entity';
import { PagedRunbookTemplate } from '../entities/paged-runbook-template.entity';
import { RunbookStep }          from '../entities/runbook-step.entity';
import { Contact }              from '../entities/contact.entity';

import { TemplateService }      from '../templates/template.service';
import { UserService }          from '../services/user.service';

import { Subscription }         from 'rxjs/Subscription';

// ---

import { TypeaheadMatch }       from 'ng2-bootstrap/components/typeahead/typeahead-match.class';

import { FormControl }          from '@angular/forms';
import { FormGroup }            from '@angular/forms';

import { Observable }           from 'rxjs/Observable';

import 'rxjs/add/observable/of';

@Component({
    templateUrl: 'app/rfcs/rfc-add.component.html',
    providers:   []
})

export class RfcAddComponent implements OnInit {

    Title = "Add RFC";

    runningSearch: boolean;

    runbookTemplates: RunbookTemplate[];
    searchResults: RunbookTemplate[];
    delaySearch: boolean;
    searchTemplateName: string;

    selectedRunbookTemplate: RunbookTemplate;

    private subscription: Subscription;

    private isSelectTemplateHidden: boolean = true;

    constructor(private templateService: TemplateService, private messageService: MessageService, private userService: UserService,  private router: Router) {

        this.contacts = Observable.create((observer: any) => {
            // Runs on every search
            observer.next(this.userTypedContactName);
        }).mergeMap((token: string) => this.getContactsAsObservable(token));
    }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    // -------------------------------------------------------------------------------------------------------------------------

    // TEMPLATES

    searchTemplateNameChanged(newValue): void {

        this.delaySearch = true;

        if (newValue !== '') {
            this.searchTemplateName = newValue;
            this.searchResults = this.runbookTemplates.filter(item => item.Name.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
    }

    selectRunbookTemplate(selectedRunbookTemplate: RunbookTemplate): void {
        this.closeTemplateSelection();
        this.selectedRunbookTemplate = selectedRunbookTemplate;
        this.getRunbookTemplate();
    }

    clearSelectedTemplateClicked(): void {
        this.selectedRunbookTemplate = null;
        this.runbookTemplate = null;
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

    listRfcsClick() {
        this.router.navigate(['rfcs']);
    }


    // -------------------------------------------------------------------------------------------------------------------------

    isNotCollapsed: boolean;

    runbookTemplate: RunbookTemplate;

    // UI Events

    openTemplateSelection() {
        this.isSelectTemplateHidden = false;
    }

    closeTemplateSelection() {
        this.isSelectTemplateHidden = true;
    }

    toggleCollapseAll() {
        this.isNotCollapsed = !this.isNotCollapsed;
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

    private getRunbookTemplate(): void {
        let id = this.selectedRunbookTemplate.ID;
        this.templateService.getRunbookTemplate(id)
            .subscribe(response => this.getRunbookTemplateOnSuccess(response), response => this.getRunbookTemplateOnError(response));
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

    // -------------------------------------------------------------------------------------------------------------------------

    // Contact Typeahead

    contact: Contact;                        // Search Request
    contacts: Observable<any>;               // Search Response
    selectedContact: Contact;                // Selected Contact

    typeaheadLoading: boolean = false;       // For showing indicator message in the UI
    typeaheadNoResults: boolean = false;     // For showing indicator message in the UI

    userTypedContactName: string = '';

    clearSelectedContactClicked(): void {
        this.userTypedContactName = '';
        this.contact = null;
        this.selectedContact = null;
    }

    getContactsAsObservable(token: string): Observable<any> {

        this.selectedContact = null;

        let contact = new Contact();
        contact.DisplayName = token;
        return this.userService.getContactsObservable(contact);
    }

    changeTypeaheadLoading(e: boolean): void {
        this.typeaheadLoading = e;
    }

    changeTypeaheadNoResults(e: boolean): void {
        this.typeaheadNoResults = e;
    }

    typeaheadOnSelect(e: TypeaheadMatch): void {
        console.log('SELECTED VALUE: ', e.item.item);
        this.selectedContact = e.item.item;
    }
}