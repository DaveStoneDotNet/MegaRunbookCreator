
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { AfterViewInit }        from '@angular/core';
import { EventEmitter }         from '@angular/core';
import { ViewChild }            from '@angular/core';
import { ViewChildren }         from '@angular/core';
import { ElementRef }           from '@angular/core';
import { Renderer }             from '@angular/core';
import { QueryList }            from '@angular/core';

import { Router }               from '@angular/router';

import { RfcService }           from './rfc.service';

import { MessageService }       from '../services/message.service';

import { RunbookTemplate }      from '../entities/runbook-template.entity';
import { PagedRunbookTemplate } from '../entities/paged-runbook-template.entity';
import { RunbookStep }          from '../entities/runbook-step.entity';
import { Contact }              from '../entities/contact.entity';
import { ServiceResponse }      from '../entities/service-response.entity';

import { TemplateService }      from '../templates/template.service';
import { UserService }          from '../services/user.service';

import { TimePickerInfo }       from '../common/timepicker/timepicker.entity';
import { DurationPickerInfo }   from '../common/timepicker/durationpicker.entity';

import { MrcFocusDirective }   from '../common/mrc-focus.directive';

import { RFC }                  from '../entities/rfc.entity';

import { Subscription }         from 'rxjs/Subscription';

// ---

import { TypeaheadMatch }       from 'ng2-bootstrap/components/typeahead/typeahead-match.class';

import { FormControl }          from '@angular/forms';
import { FormGroup }            from '@angular/forms';

import { Observable }           from 'rxjs/Observable';

import * as moment              from 'moment';

import 'rxjs/add/observable/of';

@Component({
    templateUrl: 'app/rfcs/rfc-add.component.html',
    providers: []
})

export class RfcAddComponent implements OnInit, OnDestroy, AfterViewInit {

    Title = "Add RFC";

    runningSearch: boolean;

    runbookTemplates: RunbookTemplate[];
    searchResults: RunbookTemplate[];
    delaySearch: boolean;
    searchTemplateName: string;

    selectedRunbookTemplate: RunbookTemplate;

    focusSettingEventEmitter = new EventEmitter<boolean>();

    private subscription: Subscription;

    private isSelectTemplateHidden: boolean = true;

    @ViewChild('RfcNumber') rfcNumberInputElement: ElementRef;
    @ViewChildren('RfcNumber') children: QueryList<any>;

    constructor(private templateService: TemplateService, private messageService: MessageService, private userService: UserService, private rfcService: RfcService, private router: Router, private renderer: Renderer) {

        this.newRfc();

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

    ngAfterViewInit() {

        // This Works...
        this.children.changes.subscribe((comps: QueryList<any>) => {
            this.rfcNumberInputElement = comps.first;
            if (this.rfcNumberInputElement) {
                this.renderer.invokeElementMethod(this.rfcNumberInputElement.nativeElement, 'focus');
            }
        });


        // This doesn't work... it's always UNDEFINED...
        if (this.rfcNumberInputElement) {
            this.renderer.invokeElementMethod(this.rfcNumberInputElement.nativeElement, 'focus');
        }

        // This doesn't work either, when running as a directive. Not sure why...
        this.focusSettingEventEmitter.emit(true);
    }

    setFocus(): void {
        this.focusSettingEventEmitter.emit(true);
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

    clearSelectedTemplateClicked(template: RunbookTemplate): void {

        //let item = this.rfc.Templates.filter(x => x.ID === template.ID)[0];

        //let exists = item === undefined ? false : true;

        let index = -1;

        for (let i = 0; i < this.rfc.Templates.length; i++) {
            if (this.rfc.Templates[i].ID === template.ID) {
                index = i;
                break;
            }
        }

        if (index >= 0) {
            this.rfc.Templates.splice(index, 1);
        }

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

    insertRfcClick() {

        this.messageService.sendTextMessage('Searching...');

        this.rfc.StartTime = moment(this.rfc.StartTimeText, 'hh:mm A').year(2000).month(0).day(0).toDate();
        this.rfc.EndTime = moment(this.rfc.EndTimeText, 'hh:mm A').year(2000).month(0).day(0).toDate();

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.subscription = this.rfcService.insertRfc(this.rfc)
                .subscribe(
                response => this.saveRfcOnSuccess(response),
                response => this.saveRfcOnError(response)
                );
        },
            miliseconds);
    }

    private saveRfcOnSuccess(response: ServiceResponse): void {

        console.log('SAVE SUCCESS');
        this.runningSearch = false;
        this.messageService.sendTextMessage('Ready');
    }

    private saveRfcOnError(response): void {

        console.log('SAVE ERROR');
        this.runningSearch = false;
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

        if (this.rfc.Name.length === 0) {
            this.rfc.Name = response.Name;
        }

        let item = this.rfc.Templates.filter(x => x.ID === response.ID)[0];

        let exists = item === undefined ? false : true;

        if (!exists) {
            this.runbookTemplate = response;
            this.rfc.Templates.push(this.runbookTemplate);
        }

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

    typeaheadOnSelect(e: any): void {
        console.log('SELECTED VALUE: ', e.item.item);
        this.selectedContact = e.item.item;
        this.rfc.Contact = e.item.item;
    }

    // -------------------------------------------------------------------------------------------------------------------------

    // TIME PICKER

    selectedStartTime: TimePickerInfo = new TimePickerInfo();
    selectedEndTime:   TimePickerInfo = new TimePickerInfo();
    selectedDuration: DurationPickerInfo = new DurationPickerInfo();

    onStartTimeInputChanged(timePickerInfo: TimePickerInfo): void {

        console.log(timePickerInfo.TimeText + ' START CHANGED');

        this.rfc.StartTimeText = timePickerInfo.TimeText;
        this.selectedStartTime = timePickerInfo;

        this.addDurationToStart(this.selectedDuration);
    }

    onEndTimeInputChanged(timePickerInfo: TimePickerInfo): void {

        console.log(timePickerInfo.TimeText + ' END CHANGED');

        this.rfc.EndTimeText = timePickerInfo.TimeText;
        this.selectedEndTime = timePickerInfo;

        this.selectedDuration = new DurationPickerInfo();
    }

    onStartTimeSelected(timePickerInfo: TimePickerInfo): void {

        console.log(timePickerInfo.TimeText + ' START SELECTED');

        this.rfc.StartTimeText = timePickerInfo.TimeText;
        this.selectedStartTime = timePickerInfo;

        this.addDurationToStart(this.selectedDuration);
    }

    onDurationSelected(duration: DurationPickerInfo): void {

        this.selectedDuration = duration;

        console.log(this.selectedDuration.DurationText + ' DURATION SELECTED');

        this.addDurationToStart(duration);
    }

    addDurationToStart(duration: DurationPickerInfo) {

        if (this.hasDuration(duration)) {

            if (this.hasTime(this.selectedStartTime)) {
                this.selectedEndTime = this.getTime(this.selectedStartTime);
                if (this.selectedEndTime) {
                    this.selectedEndTime.MomentValue.add(this.selectedDuration.Hours, 'hours');
                    this.selectedEndTime.MomentValue.add(this.selectedDuration.Minutes, 'minutes');

                    // TimeText is what's bound to the INPUT. The other "INTERNALS" of the COMPONENT are bound to the selectedEndTime.
                    this.selectedEndTime.TimeText = this.selectedEndTime.MomentValue.format('hh:mm A');
                    this.rfc.EndTimeText = this.selectedEndTime.TimeText;
                }
            }
        }
    }

    subtractDurationFromEnd(duration: DurationPickerInfo) {

        if (duration) {
            if (this.selectedEndTime) {
                this.selectedStartTime = this.getTime(this.selectedEndTime);
                if (this.selectedStartTime.MomentValue) {
                    this.selectedStartTime.MomentValue.subtract(this.selectedDuration.Hours, 'hours');
                    this.selectedStartTime.MomentValue.subtract(this.selectedDuration.Minutes, 'minutes');
                    this.selectedStartTime.TimeText = this.selectedStartTime.MomentValue.format('hh:mm A');
                    this.rfc.StartTimeText = this.selectedStartTime.TimeText;
                }
            }
        }
    }

    getTime(timePickerInfo: TimePickerInfo): TimePickerInfo {

        let t = new TimePickerInfo();

        timePickerInfo.Hour = timePickerInfo.Hour ? timePickerInfo.Hour : 0;
        timePickerInfo.Minute = timePickerInfo.Minute ? timePickerInfo.Minute : 0;

        let m = moment().hour(timePickerInfo.Hour).minute(timePickerInfo.Minute);
        t.MomentValue = m;
        t.TimeValue = m.toDate();
        t.TimeText = m.format('hh:mm A');
        t.Hour = timePickerInfo.Hour;
        t.Minute = timePickerInfo.Minute;
        t.Second = 0;

        return t;
    }

    hasTime(timePickerInfo: TimePickerInfo): boolean {

        let b = false;

        if (timePickerInfo) {
            if (timePickerInfo.Hour !== undefined && timePickerInfo.Minute !== undefined) {
                b = true;
            }
        }

        return b;
    }

    hasDuration(duration: DurationPickerInfo): boolean {

        let b = false;

        if (duration) {
            if (duration.Hours !== undefined && duration.Minutes !== undefined) {
                b = true;
            }
        }

        return b;
    }

    // -------------------------------------------------------------------------------------------------------------------------

    // FORMS

    active = true;
    submitted = false;

    rfc: RFC = new RFC();

    onSubmit(form: any) {
        this.submitted = true;
        console.log(form.name);
    }

    newRfc(): void {

        this.rfc = new RFC();

        this.rfc.Templates = new Array<RunbookTemplate>();

        // Reset the form with a new hero and restore the 'pristine' state by toggling the 'active' flag.
        // The NgIf reference in the HTML causes the form to be removed from the DOM and then immediately re-added 
        // which has the effect of changing the form state back to 'pristine'.

        // This a temporary hack until a proper form reset feature is added to Angular.

        this.active = false;
        setTimeout(() => this.active = true, 0);
    }

    get diagnostic() { return JSON.stringify(this.rfc); }

}