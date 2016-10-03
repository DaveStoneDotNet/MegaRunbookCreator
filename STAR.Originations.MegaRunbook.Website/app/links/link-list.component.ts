
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { OnDestroy }       from '@angular/core';

import { Router }          from '@angular/router';
import { ActivatedRoute }  from '@angular/router';

import { EnvironmentLink } from '../entities/environment-link.entity';
import { ApplicationLink } from '../entities/application-link.entity';
import { LinkService }     from './link.service';

import { Subscription }    from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/links/link-list.component.html',
    providers: [LinkService]
})

export class LinkListComponent implements OnInit, OnDestroy {

    Title = "Links";

    searchCriteria: string;

    applicationLink: ApplicationLink;

    environmentLink: EnvironmentLink;

    delaySearch: boolean;
    runningSearch: boolean;
    searchResults: ApplicationLink[];

    private selectedId: number;
    private subscription: Subscription;

    constructor(private router: Router, private route: ActivatedRoute, private service: LinkService) { }

    ngOnInit() {
        this.subscription = this.route
            .params
            .subscribe(params => {
                this.selectedId = +params['id'];
                this.service.getApplicationLinks()
                    .then(searchResults => this.onServiceLinksSuccessful(searchResults));
            });
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
            this.searchResults = this.searchResults.filter(item => item.FolderName.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
    }

    onClearSearchCriteria(): void {
        this.searchCriteria = '';
        this.onSearch('');
    }

    onSelect(applicationLink: ApplicationLink) {
        this.applicationLink = applicationLink;
    }

    onSelectedEnvironment(environmentLink: EnvironmentLink) {
        this.environmentLink = environmentLink;
    }

    isSelected(applicationLink: ApplicationLink) {
        return applicationLink.ID === this.selectedId;
    }

    private onServiceLinksSuccessful(response: ApplicationLink[]) {
        this.searchResults = response;
        this.runningSearch = false;
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        let miliseconds = 500;

        if (this.delaySearch === false) {
            miliseconds = 0;
        }

        this.runningSearch = true;

        setTimeout(() => {

            this.service.getApplicationLinks()
                .then(searchResults => this.onServiceLinksSuccessful(searchResults));
        },
            miliseconds);
    }

}