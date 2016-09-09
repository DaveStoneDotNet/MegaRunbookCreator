
import { Injectable }   from '@angular/core';
import { EventEmitter } from '@angular/core';

@Injectable()
export class BlockUIService {

    blockUIEvent: EventEmitter<Object>;

    constructor() {
        this.blockUIEvent = new EventEmitter();
    }

    startBlock() {
        this.blockUIEvent.emit(true);
    }

    stopBlock() {
        this.blockUIEvent.emit(false);
    }
}