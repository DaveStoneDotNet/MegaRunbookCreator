
import { Component }                 from '@angular/core';
import { OnInit }                    from '@angular/core';
import { OnDestroy }                 from '@angular/core';

import { IsWorkingService }          from './services/is-working.service';
import { IsWorkingComponent }        from './common/is-working.component';
import { IsWorkingSpinnerComponent } from './common/is-working-spinner.component';

@Component({
    selector:    'mrc-app',
    templateUrl: 'app/app.component.html' 
})

export class AppComponent implements OnInit, OnDestroy {

    today = new Date();

    isWorking = false;
    workingMessage: string;
    workingMessageSubscription: any;
    isWorkingSubscription: any;

    constructor(private isWorkingService: IsWorkingService) {

    }

    ngOnInit() {
        this.workingMessageSubscription = this.isWorkingService.isWorkingEvent.subscribe(event => this.onWorking(event));
        this.isWorkingSubscription = this.isWorkingService.isWorking.subscribe(value => this.isWorking = value);
    }

    ngOnDestroy() {
        this.workingMessageSubscription.unsubscribe();
        this.isWorkingSubscription.unsubscribe();
    }

    private onWorking(message) {
        this.workingMessage = message;
    }
}
