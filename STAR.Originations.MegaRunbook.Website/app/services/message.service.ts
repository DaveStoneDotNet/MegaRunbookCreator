
import { Injectable }    from '@angular/core';
import { EventEmitter }  from '@angular/core';

import {BehaviorSubject} from 'rxjs/BehaviorSubject';

import { Message }       from '../entities/message.entity';

import 'rxjs/add/operator/share';
import 'rxjs/add/operator/startWith';

@Injectable()
export class MessageService {

    onMessageChanged: EventEmitter<Message>;

    isWorking: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    constructor() {
        this.onMessageChanged = new EventEmitter<Message>();
    }

    isDelayed = false;

    sendTextMessage(textMessage: string) {
        let message = new Message(textMessage);
        this.sendMessage(message);
    }

    sendMessage(message: Message) {

        this.isDelayed = true;

        setTimeout(() => {
            if (this.isDelayed) {
                this.isWorking.next(true);
                this.onMessageChanged.emit(message);
            }
        },
        1000);
    }
}
