
import { Component }         from '@angular/core';
import { OnInit }            from '@angular/core';
import { OnDestroy }         from '@angular/core';
import { AfterViewInit }     from '@angular/core';
import { EventEmitter }      from '@angular/core';
import { ViewContainerRef  } from '@angular/core';
import { ViewChild }         from '@angular/core';
import { ViewChildren }      from '@angular/core';
import { ElementRef }        from '@angular/core';
import { Renderer }          from '@angular/core';
import { QueryList }         from '@angular/core';

import { Router }            from '@angular/router';
import { ActivatedRoute }    from '@angular/router';

import { ToastrService }     from 'toastr-ng2';

import { IsWorkingService }  from '../services/is-working.service';
import { MappingService }    from '../services/mapping.service';
import { MessageService }    from '../services/message.service';

import { RFC }               from '../entities/rfc.entity';
import { PagedRfc }          from '../entities/paged-rfc.entity';
import { RunbookStep }       from '../entities/runbook-step.entity';

import { RfcService }        from './rfc.service';

import { MrcFocusDirective } from '../common/mrc-focus.directive';

import { DataTable }         from '../common/datatable/mrc-datatable.directive';
import { DataEvent }         from '../common/datatable/i-data-event';

import { Paging }            from '../entities/paging.entity';
import { SortInfo }          from '../entities/sort-info.entity';

import { Subscription }      from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/rfcs/rfc-list.component.html',
    providers:   [MappingService]
})

export class RfcListComponent implements OnInit, OnDestroy, AfterViewInit {

    Title = "RFC's";

    @ViewChildren('mrcDataTable') myMrcDataTableElement: DataTable;
    @ViewChild('RfcSearchInput') rfcNumberInputElement: ElementRef;
    @ViewChildren('RfcSearchInput') children: QueryList<any>;

    runningSearch: boolean;

    searchCriteria = new RFC();

    selectedRfc: RFC;
    rfcs: RFC[];
    searchResults: RFC[];
    delaySearch: boolean;
    searchTemplateName: string;

    pagedRfcs: PagedRfc;

    focusSettingEventEmitter = new EventEmitter<boolean>();

    private subscription: Subscription;
    private dataEvent: DataEvent;

    constructor(private router: Router,
                private route: ActivatedRoute,
                private isWorkingService: IsWorkingService,
                private toastrService: ToastrService,
                private mappingService: MappingService, 
                private messageService: MessageService, 
                private rfcService: RfcService,
                private viewContainerRef: ViewContainerRef,
                private renderer: Renderer) { }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    ngAfterViewInit() {

        // This doesn't work...
        // ... if the parent container is NOT *ngIF
        this.children.changes.subscribe((comps: QueryList<any>) => {
            console.log('MONKEY B')
            this.rfcNumberInputElement = comps.first;
            if (this.rfcNumberInputElement) {
                console.log('MONKEY B-01')
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

    onRfcNameChanged(newValue): void {

        this.searchCriteria.Name = newValue;
        this.executeSearch();
    }

    onClearSearchCriteria(): void {

        this.searchCriteria.Name = '';
        this.executeSearch();
    }

    onRfcSelected(rfc: RFC) {
        this.getRfc(rfc);
    }

    searchNameChanged(newValue): void {

        this.delaySearch = true;

        if (newValue !== '') {
            this.searchTemplateName = newValue;
            this.searchResults = this.rfcs.filter(item => item.Name.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
    }

    onDataRequested($event): void {
        this.dataEvent = $event;
        this.executeSearch();
    }

    private getRequest(): RFC {

        let request = this.searchCriteria ? this.searchCriteria : new RFC();

        let recordsPerPage = 10;

        if (this.myMrcDataTableElement) {
            recordsPerPage = this.myMrcDataTableElement.recordsPerPage;
        }

        request.Paging = this.mappingService.dataEventToPaging(this.dataEvent, recordsPerPage);

        return request;
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
            let request = this.getRequest();
            this.subscription = this.rfcService.getRfcs(request)
                .subscribe(
                response => this.getRfcsOnSuccess(response),
                response => this.getRfcsOnError(response)
                );
        },
            miliseconds);
    }

    private getRfcsOnSuccess(response: PagedRfc): void {

        let message = response.TotalRecordCount + (response.TotalRecordCount === 1 ? ' application' : ' applications');

        this.pagedRfcs = response;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
        this.messageService.sendTextMessage('READY');
    }

    private getRfcsOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    // ---

    private getRfc(rfc: RFC): void {

        this.messageService.sendTextMessage('Getting RFC...');

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {
            this.subscription = this.rfcService.getRfc(rfc.Id)
                .subscribe(
                response => this.getRfcOnSuccess(response),
                response => this.getRfcOnError(response)
                );
        },
            miliseconds);
    }

    private getRfcOnSuccess(response: RFC): void {

        this.selectedRfc = response;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
        this.messageService.sendTextMessage('READY');
    }

    private getRfcOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    // ---

    addRfcClick() {
        this.router.navigate(['add-rfc']);
    }

    editRfcClick(rfc: RFC) {
        this.router.navigate(['edit-rfc', rfc.Id]);
    }

    // -------------------------------------------------------------------------------------------------------------------------

    isNotCollapsed: boolean;

    toggleCollapseAll() {
        this.isNotCollapsed = !this.isNotCollapsed;
        if (this.selectedRfc && this.selectedRfc.RunbookSteps) {
            this.selectedRfc.RunbookSteps.forEach(s => s.IsNotCollapsed = this.isNotCollapsed);
        }
    }

    toggleCollapse(step: RunbookStep) {
        step.IsNotCollapsed = !step.IsNotCollapsed;
        this.syncCollapsables();
    }

    private syncCollapsables() {

        // If *ALL* of the collapsibles are collapsed then sync the 'global' IsNotCollapsed to collapsed.
        // If *ALL* of the collapsibles are NOT collapsed then sync the 'global' IsNotCollapsed to not collapsed.

        let numerics: number[] = new Array(this.selectedRfc.RunbookSteps.length);
        for (var i = 0; i < this.selectedRfc.RunbookSteps.length; i++) {
            if (this.selectedRfc.RunbookSteps[i].IsNotCollapsed) {
                numerics[i] = 1;
            } else {
                numerics[i] = 0;
            }
        }

        let is: number = 0;
        let isnot: number = 0;
        numerics.forEach(n => { if (n == 0) isnot = isnot + 1 });
        numerics.forEach(n => { if (n == 1) is = is + 1 });

        if (isnot == this.selectedRfc.RunbookSteps.length) {
            this.isNotCollapsed = false;
        }
        if (is == this.selectedRfc.RunbookSteps.length) {
            this.isNotCollapsed = true;
        }
    }

}