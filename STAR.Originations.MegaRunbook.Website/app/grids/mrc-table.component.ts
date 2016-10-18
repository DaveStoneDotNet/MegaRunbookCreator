
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { ViewChild }            from '@angular/core';
import { Router }               from '@angular/router';

import { ApplicationLink }      from '../entities/application-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';
import { LinkService }          from '../links/link.service';
import { IsWorkingService }     from '../services/is-working.service';
import { MappingService }       from '../services/mapping.service';

import { DataEvent }            from '../common/datatable/i-data-event';
import { Paging }               from '../entities/paging.entity';
import { SortInfo }             from '../entities/sort-info.entity';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl:  'app/grids/mrc-table.component.html',
    styleUrls:   ['app/grids/mrc-table.component.css'],
    providers:   [LinkService, MappingService]
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

    constructor(private linkService: LinkService, private isWorkingService: IsWorkingService, private mappingService: MappingService) {

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

        let request = new ApplicationLink();

        request.Paging = this.mappingService.dataEventToPaging(this.dataEvent, this.myMrcDataTableElement.recordsPerPage);

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

        this.pagedApplicationLinks = response;

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }
}