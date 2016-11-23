
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { ActivatedRoute }  from '@angular/router';
import { Params }          from '@angular/router';
import { Router }          from '@angular/router';

import { RFC }             from '../entities/rfc.entity';
import { RunbookStep }     from '../entities/runbook-step.entity';
import { Contact }         from '../entities/contact.entity';

import { UserService }     from '../services/user.service';
import { RfcService }      from './rfc.service';

import { Observable }      from 'rxjs/Observable';

@Component({
    templateUrl: 'app/rfcs/rfc-edit.component.html',
    styleUrls:  ['app/rfcs/rfc-edit.component.css'], 
    providers:  []
})

export class RfcEditComponent implements OnInit {

    Title = "Edit RFC";

    active = true;
    submitted = false;

    isEditing: boolean = false;

    selectedRunbookStep: RunbookStep;

    runningSearch: boolean;
    isNotCollapsed: boolean;

    rfc: RFC;

    constructor(private rfcService: RfcService,
                private route: ActivatedRoute,
                private userService: UserService,
                private router: Router) {
        
        this.contacts = Observable.create((observer: any) => {
            // Runs on every search
            observer.next(this.userTypedContactName);
        }).mergeMap((token: string) => this.getContactsAsObservable(token));
    }

    ngOnInit() {

        this.executeSearch();
    }

    // UI Events

    toggleCollapseAll() {
        this.isNotCollapsed = !this.isNotCollapsed;
        if (this.rfc && this.rfc.RunbookSteps) {
            this.rfc.RunbookSteps.forEach(s => s.IsNotCollapsed = this.isNotCollapsed);
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
        //let link = ['/runbookStepForm', step.Id];
        //this.router.navigate(link);
        this.isEditing = true;
        this.selectedRunbookStep = step;
    }

    hideEditing() {
        this.isEditing = false;
        this.selectedRunbookStep = null;
    }
    // Data Access

    private executeSearch(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            this.rfcService.getRfc(id)
                .subscribe(response => this.getRunbookTemplateOnSuccess(response), response => this.getRunbookTemplateOnError(response));
        });
    }

    private getRunbookTemplateOnSuccess(response: RFC): void {

        this.rfc = response;

        this.runningSearch = false;
    }

    private getRunbookTemplateOnError(response): void {

        this.runningSearch = false;
    }

    // Helper / Other

    private syncCollapsables() {

        // If *ALL* of the collapsibles are collapsed then sync the 'global' IsNotCollapsed to collapsed.
        // If *ALL* of the collapsibles are NOT collapsed then sync the 'global' IsNotCollapsed to not collapsed.

        let numerics: number[] = new Array(this.rfc.RunbookSteps.length);
        for (var i = 0; i < this.rfc.RunbookSteps.length; i++) {
            if (this.rfc.RunbookSteps[i].IsNotCollapsed) {
                numerics[i] = 1;
            } else {
                numerics[i] = 0;
            }
        }

        let is: number = 0;
        let isnot: number = 0;
        numerics.forEach(n => { if (n == 0) isnot = isnot + 1 });
        numerics.forEach(n => { if (n == 1) is = is + 1 });

        if (isnot == this.rfc.RunbookSteps.length) {
            this.isNotCollapsed = false;
        }
        if (is == this.rfc.RunbookSteps.length) {
            this.isNotCollapsed = true;
        }
    }

    // ---

    listRfcsClick() {
        this.router.navigate(['rfcs']);
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

    typeaheadOnSelect(e: any): void {
        console.log('SELECTED VALUE: ', e.item.item);
        this.selectedContact = e.item.item;
        this.rfc.Contact = e.item.item;
    }

}