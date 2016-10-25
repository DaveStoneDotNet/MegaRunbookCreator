
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { ViewContainerRef  }    from '@angular/core';
import { ViewChild }            from '@angular/core';
import { ViewChildren }         from '@angular/core';

import { Router }               from '@angular/router';
import { ActivatedRoute }       from '@angular/router';

import { ToastrService }        from 'toastr-ng2';

import { IsWorkingService }     from '../services/is-working.service';
import { MappingService }       from '../services/mapping.service';
import { MessageService }       from '../services/message.service';
import { AppService }           from '../services/app.service';

import { LinkService }          from './link.service';

import { EnvironmentLink }      from '../entities/environment-link.entity';
import { ApplicationLink }      from '../entities/application-link.entity';
import { ServiceLink }          from '../entities/service-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';

import { DataTable }            from '../common/datatable/mrc-datatable.directive';
import { DataEvent }            from '../common/datatable/i-data-event';

import { Paging }               from '../entities/paging.entity';
import { SortInfo }             from '../entities/sort-info.entity';
import { AppLookups }           from '../entities/app-lookups.entity';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/links/link-list.component.html',
    providers:   [LinkService, MappingService]
})

export class LinkListComponent implements OnInit, OnDestroy {

    Title = "Links";

    @ViewChildren('mrcDataTable') myMrcDataTableElement: DataTable;

    searchCriteria = new ApplicationLink();

    applicationLink: ApplicationLink;
    serviceLink: ServiceLink;
    environmentLink: EnvironmentLink;

    delaySearch: boolean;
    runningSearch: boolean;
    searchResults: ApplicationLink[];

    pagedApplicationLink: PagedApplicationLink;

    appLookups: AppLookups;
    selectedApplicationGroup: AppLookups;
    selectedApplicationType: AppLookups;

    private selectedId: number;
    private subscription: Subscription;
    private dataEvent: DataEvent;

    constructor(private router: Router,
                private route: ActivatedRoute,
                private linkService: LinkService,
                private isWorkingService: IsWorkingService,
                private toastrService: ToastrService,
                private mappingService: MappingService, 
                private messageService: MessageService, 
                private appService: AppService, 
                private viewContainerRef: ViewContainerRef) {
        this.toastrService.viewContainerRef = this.viewContainerRef;
    }

    ngOnInit() {
        this.appLookups = this.appService.lookups;
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    onApplicationNameChanged(newValue): void {

        this.searchCriteria.Name = newValue;
        this.executeSearch();
    }

    onGroupChanged(newValue): void {

        this.delaySearch = true;
        this.messageService.sendTextMessage('Searching...');
        this.selectedApplicationGroup = newValue;
        this.searchCriteria.ApplicationGroupId = newValue.Code;
        this.executeSearch();
    }

    onTypeChanged(newValue): void {

        this.delaySearch = true;
        this.messageService.sendTextMessage('Searching...');
        this.selectedApplicationType = newValue;
        this.searchCriteria.ApplicationType = newValue;
        this.executeSearch();
    }

    onClearSearchCriteria(): void {

        this.messageService.sendTextMessage('Clearing...');
        this.searchCriteria = new ApplicationLink();
        this.applicationLink = null;
        this.serviceLink = null;
        this.environmentLink = null;
        this.selectedApplicationGroup = null;
        this.selectedApplicationType = null;
        this.onApplicationNameChanged('');
    }

    onApplicationLinkSelected(applicationLink: ApplicationLink) {
        if (this.applicationLink != null) {
            this.applicationLink.IsSelected = false;
        }
        this.applicationLink = applicationLink;
        this.applicationLink.IsSelected = true;
        this.serviceLink = null;
        this.environmentLink = null;
    }

    onServiceLinkSelected(serviceLink: ServiceLink) {
        if (this.serviceLink != null) {
            this.serviceLink.IsSelected = false;
        }
        this.serviceLink = serviceLink;
        this.serviceLink.IsSelected = true;
        this.environmentLink = null;
    }

    onEnvironmentSelected(environmentLink: EnvironmentLink) {
        if (this.environmentLink != null) {
            this.environmentLink.IsSelected = false;
        }
        this.environmentLink = environmentLink;
        this.environmentLink.IsSelected = true;
    }

    onSelectedEnvironmentLinkClosed() {
        this.environmentLink.IsSelected = false;
        this.environmentLink = null;
    }

    onCopiedToClipboard(msg: string) {
        this.toastrService.success('Copied ' + msg + ' to clipboard');
    }

    onDataRequested($event): void {
        this.dataEvent = $event;
        this.executeSearch();
    }

    private getRequest(): ApplicationLink {

        let request = this.searchCriteria ? this.searchCriteria : new ApplicationLink();

        let recordsPerPage = 10;

        if (this.myMrcDataTableElement) {
            recordsPerPage = this.myMrcDataTableElement.recordsPerPage;
        }

        request.Paging = this.mappingService.dataEventToPaging(this.dataEvent, recordsPerPage);

        return request;
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            let request = this.getRequest();
            this.subscription = this.linkService.getApplicationLinks(request)
                .subscribe(
                response => this.onServiceLinksSuccessful(response),
                response => this.onServiceLinksOnError(response)
            );
        },
            miliseconds);
    }

    private onServiceLinksSuccessful(response: PagedApplicationLink) {

        let message = response.TotalRecordCount + (response.TotalRecordCount === 1 ? ' application' : ' applications');

        this.pagedApplicationLink = response;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
        this.toastrService.success(message);
        this.messageService.sendTextMessage('READY');
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    enviromentBackground(): string {
        let style = '';
        if (this.environmentLink) {
            if (this.environmentLink.Server) {
                if (this.environmentLink.Server.Environment) {
                    switch (this.environmentLink.Server.Environment.Name) {
                        case 'QA1':
                        case 'QA2':
                            style = 'muted-green-bg white';
                            break;
                        case 'UAT1':
                        case 'UAT2':
                            style = 'dark-orange-bg white';
                            break;
                        case 'PROD1':
                        case 'PROD2':
                        case 'PROD3':
                        case 'PROD4':
                            style = 'soft-red-bg white';
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return style;
    }
}