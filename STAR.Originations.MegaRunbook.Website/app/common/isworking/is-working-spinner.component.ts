
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
    selector:    'is-working-spinner',
    templateUrl: 'app/common/isworking/is-working-spinner.component.html', 
    styleUrls:   ['app/common/isworking/is-working-spinner.component.css'], 

    animations: [
                    trigger('workingState', [
                                                state('popped-out', style({ transform: 'translateX(-30%)' })),
                    
                                                transition('void => *', [style({ transform: 'translateX(-50%)' }), animate(100)]),
                                                transition('* => void', [animate(100, style({ transform: 'translateX(100%)' }))])
                                            ]
                           )
                    ]
})

export class IsWorkingSpinnerComponent implements OnInit, OnDestroy {

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

    onClickMe() {
        if (this.state === null) {
            this.state = 'popped-out';
            console.log('STATE: ' + this.state);
        } else {
            this.state = null;
            console.log('STATE: NULL');
        }
    }

    private onIsWorking(message) {
        this.workingMessage = message;
    }
}