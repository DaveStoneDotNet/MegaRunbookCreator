﻿
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';

import { Router }               from '@angular/router';
import { ActivatedRoute }       from '@angular/router';

import { EnvironmentLink }      from '../entities/environment-link.entity';
import { ApplicationLink }      from '../entities/application-link.entity';
import { ServiceLink }          from '../entities/service-link.entity';
import { PagedApplicationLink } from '../entities/paged-application-link.entity';
import { LinkService }          from './link.service';

import { Subscription }         from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/links/link-list.component.html',
    providers: [LinkService]
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

    constructor(private router: Router, private route: ActivatedRoute, private linkService: LinkService) { }

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
        this.onSearch('');
    }

    onApplicationLinkSelected(applicationLink: ApplicationLink) {
        this.applicationLink = applicationLink;
        this.serviceLink = null;
    }

    onServiceLinkSelected(serviceLink: ServiceLink) {
        this.serviceLink = serviceLink;
    }

    onEnvironmentSelected(environmentLink: EnvironmentLink) {
        this.environmentLink = environmentLink;
    }

    isSelected(applicationLink: ApplicationLink) {
        return applicationLink.Id === this.selectedId;
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

        this.pagedApplicationLink = response;


        this.runningSearch = false;
    }

    private onServiceLinksOnError(response): void {

        this.runningSearch = false;
    }
}