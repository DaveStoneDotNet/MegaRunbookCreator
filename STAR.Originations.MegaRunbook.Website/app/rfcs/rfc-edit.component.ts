
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { ActivatedRoute }  from '@angular/router';
import { Params }          from '@angular/router';
import { Router }          from '@angular/router';

import { RFC }             from '../entities/rfc.entity';
import { RunbookStep }     from '../entities/runbook-step.entity';
import { Contact }         from '../entities/contact.entity';
import { AppLookups }      from '../entities/app-lookups.entity';
import { Team }            from '../entities/team.entity';
import { ServiceResponse } from '../entities/service-response.entity';

import { MessageService }  from '../services/message.service';

import { AppService }      from '../services/app.service';
import { UserService }     from '../services/user.service';
import { RfcService }      from './rfc.service';

import { Subscription }    from 'rxjs/Subscription';

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
    delaySearch: boolean;

    selectedRunbookStep: RunbookStep;

    runningSearch: boolean;
    isNotCollapsed: boolean;

    rfc: RFC;

    visibility = {
        editStep: false, 
        descriptionCount: false
    };

    appLookups: AppLookups;

    private subscription: Subscription;

    constructor(private rfcService: RfcService,
                private messageService: MessageService,
                private route: ActivatedRoute,
                private userService: UserService,
                private appService: AppService, 
                private router: Router) {
        
        this.contacts = Observable.create((observer: any) => {
            // Runs on every search
            observer.next(this.userTypedDeveloperName);
        }).mergeMap((token: string) => this.getContactsAsObservable(token));
    }

    ngOnInit() {

        this.appLookups = this.appService.lookups;
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

    // -------------------------------------------------------------------------------------------------------------------------

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

    updateRunbookStepClick() {

        this.messageService.sendTextMessage('Searching...');

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.subscription = this.rfcService.updateRunbookStep(this.selectedRunbookStep)
                .subscribe(
                response => this.saveRunbookStepOnSuccess(response),
                response => this.saveRunbookStepOnError(response)
                );
        },
            miliseconds);
    }

    private saveRunbookStepOnSuccess(response: ServiceResponse): void {

        console.log('SAVE SUCCESS');
        this.runningSearch = false;
        this.messageService.sendTextMessage('Ready');
    }

    private saveRunbookStepOnError(response): void {

        console.log('SAVE ERROR');
        this.runningSearch = false;
    }

    // -------------------------------------------------------------------------------------------------------------------------

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

    // Developer Typeahead

    contacts: Observable<any>;               // Search Response
    selectedDeveloper: Contact;              // Selected Developer

    typeaheadLoading: boolean = false;       // For showing indicator message in the UI
    typeaheadNoResults: boolean = false;     // For showing indicator message in the UI

    userTypedDeveloperName: string = '';

    clearSelectedContactClicked(): void {
        this.userTypedDeveloperName = '';
        this.selectedDeveloper = null;
    }

    getContactsAsObservable(token: string): Observable<any> {

        this.selectedDeveloper = null;

        let contact = new Contact();
        contact.DisplayName = token;
        return this.userService.getContactsObservable(contact);
    }

    developerTypeaheadLoading(e: boolean): void {
        this.typeaheadLoading = e;
    }

    developerTypeaheadNoResults(e: boolean): void {
        this.typeaheadNoResults = e;
    }

    developerTypeaheadOnSelect(e: any): void {

        console.log('SELECTED VALUE: ', e.item.item);
        this.selectedDeveloper = e.item.item;


        let exists: boolean = false;

        for (var i = 0; i < this.selectedRunbookStep.Developers.length; i++) {
            if (this.selectedRunbookStep.Developers[i].Id === this.selectedDeveloper.Id) {
                exists = true;
                break;
            }
        }

        if (!exists) {
            this.selectedRunbookStep.Developers.push(this.selectedDeveloper);
        }

        this.rfc.Contact = e.item.item;
    }

    // -------------------------------------------------------------------------------------------------------------------------

    // Description

    onDescriptionHasFocused() {
        this.visibility.descriptionCount = true;
    }

    onDescriptionHasBlurred() {
        this.visibility.descriptionCount = false;
    }

    // -------------------------------------------------------------------------------------------------------------------------

    // Team

    selectedTeam: Team;

    onTeamChanged(e: any) {

        let exists: boolean = false;

        let team = new Team();
        team.Id = +e.target.selectedOptions[0].value;
        team.Name = e.target.selectedOptions[0].text;

        for (var i = 0; i < this.selectedRunbookStep.Teams.length; i++) {
            if (this.selectedRunbookStep.Teams[i].Id === team.Id) {
                exists = true;
                break;
            } 
        }

        if (!exists) {
            if (team.Id > 0) {
                this.selectedRunbookStep.Teams.push(team);
            }
        }

        this.selectedTeam = null;
    }

    onTeamCleared(team: any) {

        let exists: boolean = false;
        let index: number = 0;

        for (var i = 0; i < this.selectedRunbookStep.Teams.length; i++) {
            if (this.selectedRunbookStep.Teams[i].Id === team.Id) {
                exists = true;
                index = i;
                break;
            }
        }

        if (exists) {
            this.selectedRunbookStep.Teams.splice(index, 1);
        }
    }
}