
import { Component }             from '@angular/core';
import { OnInit }                from '@angular/core';
import { OnDestroy }             from '@angular/core';
import { Router }                from '@angular/router';

import { ApplicationLink }      from '../entities/application-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';
import { LinkService }          from '../links/link.service';
import { IsWorkingService }     from '../services/is-working.service';

import { Subscription }    from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/builds/builds.component.html',
    providers:   [LinkService]
})
export class BuildsComponent implements OnInit, OnDestroy {

    Title = "Builds";

    delaySearch: boolean;
    runningSearch: boolean;
    applicationLinks: ApplicationLink[];
    pagedApplicationLinks: PagedApplicationLink;

    private subscription: Subscription;

    private sub: Subscription;

    constructor(private linkService: LinkService, private isWorkingService: IsWorkingService) {

    }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            let request = new ApplicationLink();

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