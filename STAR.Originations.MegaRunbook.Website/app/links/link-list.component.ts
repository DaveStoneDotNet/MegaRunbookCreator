
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { ViewContainerRef  }    from '@angular/core';

import { Router }               from '@angular/router';
import { ActivatedRoute }       from '@angular/router';

import { ToastrService }        from 'toastr-ng2';

import { IsWorkingService }     from '../services/is-working.service';

import { EnvironmentLink }      from '../entities/environment-link.entity';
import { ApplicationLink }      from '../entities/application-link.entity';
import { ServiceLink }          from '../entities/service-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';

import { LinkService }          from './link.service';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/links/link-list.component.html',
    providers:   [LinkService]
})

export class LinkListComponent implements OnInit, OnDestroy {

    Title = "Links";

    searchCriteria: string;

    applicationLink: ApplicationLink;
    serviceLink: ServiceLink;
    environmentLink: EnvironmentLink;

    delaySearch: boolean;
    runningSearch: boolean;
    searchResults: ApplicationLink[];

    pagedApplicationLink: PagedApplicationLink;

    private selectedId: number;
    private subscription: Subscription;

    constructor(private router: Router,
                private route: ActivatedRoute,
                private linkService: LinkService,
                private isWorkingService: IsWorkingService,
                private toastrService: ToastrService,
                private viewContainerRef: ViewContainerRef) {
        this.toastrService.viewContainerRef = this.viewContainerRef;
    }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    counter = 0;

    onSearch(newValue): void {

        this.counter = this.counter + 1;
        console.log('SEARCHING:' + this.counter);

        this.delaySearch = true;

        if (newValue !== '') {
            this.searchCriteria = newValue;
            this.searchResults = this.searchResults.filter(item => item.Name.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
    }

    onClearSearchCriteria(): void {
        this.searchCriteria = '';
        this.applicationLink = null;
        this.serviceLink = null;
        this.environmentLink = null;
        this.onSearch('');
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

    private executeSearch(): void {

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.subscription = this.linkService.getApplicationLinks()
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
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
        this.isWorkingService.stopWorking();
    }
}