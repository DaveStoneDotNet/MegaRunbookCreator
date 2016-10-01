
import { Component }        from '@angular/core';
import { OnInit }           from '@angular/core';
import { OnDestroy }        from '@angular/core';

import { IsWorkingService } from '../services/is-working.service';

@Component({
    selector: 'is-working',
    templateUrl: 'app/common/is-working.component.html' 
})

export class IsWorkingComponent implements OnInit, OnDestroy {

    isWorking = false;
    workingMessage: string;
    workingMessageSubscription: any;
    isWorkingSubscription: any;

    constructor(private isWorkingService: IsWorkingService) {

    }

    ngOnInit() {
        this.workingMessageSubscription = this.isWorkingService.isWorkingEvent.subscribe(event => this.onIsWorking(event));
        this.isWorkingSubscription = this.isWorkingService.isWorking.subscribe(value => this.isWorking = value);
    }

    ngOnDestroy() {
        this.workingMessageSubscription.unsubscribe();
        this.isWorkingSubscription.unsubscribe();
    }

    private onIsWorking(message) {
        this.workingMessage = message;
    }
}