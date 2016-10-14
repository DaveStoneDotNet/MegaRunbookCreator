
import { Component }        from '@angular/core';
import { OnInit }           from '@angular/core';
import { OnDestroy }        from '@angular/core';

import { trigger }          from '@angular/core';
import { state }            from '@angular/core';
import { style }            from '@angular/core';
import { transition }       from '@angular/core';
import { animate }          from '@angular/core';

import { IsWorkingService } from '../services/is-working.service';

@Component({
    selector:    'message',
    templateUrl: 'app/common/message.component.html',
    styleUrls:   ['app/common/message.component.css'],

    animations: [
        trigger('workingState', [
            state('popped-in',  style({ transform: 'translateX(100%)',  opacity: '0.0' })),
            state('popped-out', style({ transform: 'translateX(-100%)', opacity: '1.0' })),

            transition('popped-in  => popped-out', [animate(100)]),
            transition('popped-out => popped-in',  [animate(100)])
        ]
        )
    ]
})

export class MessageComponent implements OnInit, OnDestroy {

    state = 'popped-out';

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
        if (this.state === 'popped-in') {
            this.state = 'popped-out';
            console.log('STATE: ' + this.state);
        } else {
            this.state = 'popped-in';
            console.log('STATE: NULL');
        }
    }

    private onIsWorking(message) {
        this.workingMessage = message;
    }
}