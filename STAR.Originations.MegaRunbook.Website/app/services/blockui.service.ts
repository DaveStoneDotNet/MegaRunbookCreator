
import { Injectable }   from '@angular/core';
import { EventEmitter } from '@angular/core';

@Injectable()
export class BlockUIService {

    public blockUIEvent: EventEmitter<Object>;

    constructor() {
        this.blockUIEvent = new EventEmitter();
    }

    public startBlock() {
        this.blockUIEvent.emit(true);
    }

    public stopBlock() {
        this.blockUIEvent.emit(false);
    }
}