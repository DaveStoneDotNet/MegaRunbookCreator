
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
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
    templateUrl: 'app/grids/mrc-table.compoonent.html',
    styleUrls:  ['app/grids/mrc-table.compoonent.css'],
    providers:   [LinkService]
})
export class MrcTableComponent implements OnInit, OnDestroy {

    Title = "Builds";

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

        // $event = Object {length: 90, PageNumber: 1, PageSize: 10, SortInfo: Object}
        console.log('DATA REQUESTED - PageNumber: ' + $event.PageNumber);

        var msg = `PageNumber: ${$event.PageNumber} PageSize: ${$event.PageSize} SortInfo.SortBy: ${$event.SortInfo.sortBy} SortInfo.SortOrder: ${$event.SortInfo.sortOrder }`;
        console.log('DATA REQUESTED: ' + msg);

        this.executeSearch();
    }

    private getRequest(): ApplicationLink {
        let applicationLink = new ApplicationLink();
        if (this.dataEvent != null) {
            applicationLink.Paging = new Paging();
            applicationLink.Paging.PageNumber = this.dataEvent.PageNumber;
            applicationLink.Paging.PageSize = this.dataEvent.PageSize;

            let sortInfo = new SortInfo();
            sortInfo.PropertyName = this.dataEvent.SortInfo.PropertyName;
            sortInfo.Order = this.dataEvent.SortInfo.sortOrder;
            applicationLink.Paging.SortInfo = [];
            applicationLink.Paging.SortInfo.push(sortInfo);
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
        this.applicationLinks = this.pagedApplicationLinks.Items;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }
}