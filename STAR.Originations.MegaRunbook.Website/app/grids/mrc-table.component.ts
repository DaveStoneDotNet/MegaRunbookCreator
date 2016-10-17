
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { ViewChild }            from '@angular/core';
import { Router }               from '@angular/router';

import { ApplicationLink }      from '../entities/application-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';
import { LinkService }          from '../links/link.service';
import { IsWorkingService }     from '../services/is-working.service';

import { DataEvent }            from '../common/datatable/i-data-event';
import { Paging }               from '../entities/paging.entity';
import { SortInfo }             from '../entities/sort-info.entity';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl:  'app/grids/mrc-table.component.html',
    styleUrls:   ['app/grids/mrc-table.component.css'],
    providers:   [LinkService]
})
export class MrcTableComponent implements OnInit, OnDestroy {

    Title = "Builds";

    @ViewChild('mrcDataTable') myMrcDataTableElement;

    delaySearch: boolean;
    runningSearch: boolean;
    applicationLinks: ApplicationLink[];
    pagedApplicationLinks: PagedApplicationLink;

    private subscription: Subscription;
    private dataEvent: DataEvent;

    constructor(private linkService: LinkService, private isWorkingService: IsWorkingService) {

    }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    onDataRequested($event): void {
        this.dataEvent = $event;
        this.executeSearch();
    }

    private getRequest(): ApplicationLink {

        let applicationLink = new ApplicationLink();

        applicationLink.Paging = new Paging();
        if (this.dataEvent != null) {
            applicationLink.Paging.PageNumber = this.dataEvent.PageNumber;
            applicationLink.Paging.RecordsPerPage = this.dataEvent.RecordsPerPage;

            if (this.dataEvent.SortInfo) {
                if (!this.isNullEmptyOrWhiteSpace(this.dataEvent.SortInfo.PropertyName)) {
                    let sortInfo = new SortInfo();
                    sortInfo.PropertyName = this.dataEvent.SortInfo.PropertyName;
                    sortInfo.SortOrder = this.dataEvent.SortInfo.SortOrder;
                    applicationLink.Paging.SortInfo = [];
                    applicationLink.Paging.SortInfo.push(sortInfo);
                }
            }

        } else {
            applicationLink.Paging.PageNumber = 1;
            applicationLink.Paging.RecordsPerPage = this.myMrcDataTableElement.recordsPerPage;
        }
        return applicationLink;
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

        this.pagedApplicationLinks = response;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    private isNullEmptyOrWhiteSpace(text: string): boolean {
        return !text || text.length == 0;
    }
}