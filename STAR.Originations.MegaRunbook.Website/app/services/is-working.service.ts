
import { Injectable }    from '@angular/core';
import { EventEmitter }  from '@angular/core';

import {BehaviorSubject} from 'rxjs/BehaviorSubject';

import 'rxjs/add/operator/share';
import 'rxjs/add/operator/startWith';

@Injectable()
export class IsWorkingService {

    isWorkingEvent: EventEmitter<Object>;

    isWorking: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    constructor() {
        this.isWorkingEvent = new EventEmitter();
    }

    isDelayed = false;

    startWorking(message: string) {

        this.isDelayed = true;

        setTimeout(() => {
            if (this.isDelayed) {
                this.isWorking.next(true);
                this.isWorkingEvent.emit(message);
            }
        },
            200);
    }

    stopWorking() {

        this.isDelayed = false;

        this.isWorking.next(false);
        this.isWorkingEvent.emit('');
    }
}