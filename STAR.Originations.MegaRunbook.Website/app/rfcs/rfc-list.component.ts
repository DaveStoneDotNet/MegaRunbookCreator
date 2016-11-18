
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { OnDestroy }      from '@angular/core';
import { Router }         from '@angular/router';

import { MessageService } from '../services/message.service';

import { RFC }            from '../entities/rfc.entity';
import { PagedRfc }       from '../entities/paged-rfc.entity';

import { RfcService }     from './rfc.service';

import { Subscription }   from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/rfcs/rfc-list.component.html',
    providers:   []
})

export class RfcListComponent implements OnInit, OnDestroy {

    Title = "RFC's";

    runningSearch: boolean;

    runbookTemplates: RFC[];
    searchResults: RFC[];
    delaySearch: boolean;
    searchTemplateName: string;

    private subscription: Subscription;

    constructor(private rfcService: RfcService, private messageService: MessageService, private router: Router) { }

    ngOnInit() {
        this.executeSearch();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    searchNameChanged(newValue): void {

        this.delaySearch = true;

        if (newValue !== '') {
            this.searchTemplateName = newValue;
            this.searchResults = this.runbookTemplates.filter(item => item.Name.toLowerCase().indexOf(newValue.toLowerCase()) !== -1);
        } else {
            setTimeout(() => this.executeSearch(), 500);
        }
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
            let rfc: RFC = new RFC();
            this.subscription = this.rfcService.getRfcs(rfc)
                .subscribe(
                response => this.getRfcsOnSuccess(response),
                response => this.getRfcsOnError(response)
                );
        },
            miliseconds);
    }

    private getRfcsOnSuccess(response: PagedRfc): void {

        this.runbookTemplates = response.Items;
        this.searchResults = response.Items;

        this.runningSearch = false;
        this.messageService.sendTextMessage('Ready');
    }

    private getRfcsOnError(response): void {

        this.runningSearch = false;
    }

    addRfcClick() {
        this.router.navigate(['add-rfc']);
    }

    editRfcClick() {
        this.router.navigate(['edit-rfc']);
    }
}