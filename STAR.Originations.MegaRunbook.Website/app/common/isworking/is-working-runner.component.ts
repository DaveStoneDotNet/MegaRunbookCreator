
import { Component }        from '@angular/core';
import { OnInit }           from '@angular/core';
import { OnDestroy }        from '@angular/core';

import { trigger }          from '@angular/core';
import { state }            from '@angular/core';
import { style }            from '@angular/core';
import { transition }       from '@angular/core';
import { animate }          from '@angular/core';

import { IsWorkingService } from '../../services/is-working.service';

@Component({
    selector:    'is-working-runner',
    templateUrl: 'app/common/isworking/is-working-runner.component.html' 
})

export class IsWorkingRunnerComponent implements OnInit, OnDestroy {

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

    onComponentClicked() {
        this.isWorking = false;
    }

    private onIsWorking(message) {
        this.workingMessage = message;
    }
}