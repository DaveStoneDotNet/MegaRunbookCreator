
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

    startWorking(message: string) {
        this.isWorking.next(true);
        this.isWorkingEvent.emit(message);
    }

    stopWorking() {
        this.isWorking.next(false);
        this.isWorkingEvent.emit('');
    }
}