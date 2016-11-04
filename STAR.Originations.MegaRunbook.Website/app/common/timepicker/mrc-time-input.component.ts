
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { OnDestroy }      from '@angular/core';
import { Output }         from '@angular/core';
import { EventEmitter }   from '@angular/core';

import { trigger }        from '@angular/core';
import { state }          from '@angular/core';
import { style }          from '@angular/core';
import { transition }     from '@angular/core';
import { animate }        from '@angular/core';

import { TimePickerInfo } from './timepicker.entity';

import * as moment        from 'moment';

@Component({
    selector:    'mrc-time-input',
    templateUrl: 'app/common/timepicker/mrc-time-input.component.html',
    styleUrls:  ['app/common/timepicker/mrc-time-input.component.css']
})

export class MrcTimeInputComponent implements OnInit, OnDestroy {

    @Output() notify: EventEmitter<TimePickerInfo> = new EventEmitter<TimePickerInfo>();

    isCollapsed: boolean = true;
    isBlurred: boolean = true;
    isValid: boolean = false;
    selectedTime: TimePickerInfo;

    startTime: Date;
    endTime: Date;
    timeText: string;
    momentFormattedTimeText: string;

    validKeys: Array<string> = new Array();

    constructor() {

    }

    ngOnInit() {
        this.validKeys.push('1');
        this.validKeys.push('2');
        this.validKeys.push('3');
        this.validKeys.push('4');
        this.validKeys.push('5');
        this.validKeys.push('6');
        this.validKeys.push('7');
        this.validKeys.push('8');
        this.validKeys.push('9');
        this.validKeys.push('0');
        this.validKeys.push(':');
        this.validKeys.push(' ');
        this.validKeys.push('a');
        this.validKeys.push('A');
        this.validKeys.push('p');
        this.validKeys.push('P');
        this.validKeys.push('m');
        this.validKeys.push('M');
    }

    ngOnDestroy() {
    }

    toggleIsCollapsed() {
        this.isCollapsed = !this.isCollapsed;
    }

    onTimeSelected(time: TimePickerInfo) {
        this.selectedTime = time;
        this.isCollapsed = true;
        this.notify.emit(this.selectedTime);
    }

    getTime(hours: number, minutes: number): TimePickerInfo {
        let t = new TimePickerInfo();
        let m = moment().hour(hours).minute(minutes);
        t.MomentValue = m;
        t.TimeValue = m.toDate();
        t.TimeText = m.format('hh:mm A');
        t.Hour = hours;
        t.Minute = minutes;
        t.Second = 0;
        return t;
    }

    timeInputKeyPressed(event): void {

        let key = event.key.toUpperCase();

        if (this.validKeys.find(k => k === key)) {
            console.log('YES');
        } else {
            console.log('NOPE');
            event.preventDefault();
        }

        let m = moment(event.srcElement.value + key, 'h:m A');
        this.momentFormattedTimeText = m.format('hh:mm A');
        this.isValid = m.isValid();
        console.log('VALID: ' + event.srcElement.value + ' | ' + this.isValid + ' (' + m.format('hh:mm A') + ')');
        //console.log(event, event.keyCode, event.keyIdentifier);

    }

    timeInputFocused(event) {
        this.isBlurred = false;
    }

    timeInputBlurred(event): void {

        this.isBlurred = true;

        console.log(this.timeText);

        let m = moment(this.timeText, 'h:m A');
        let b = m.isValid();
        console.log('VALID: ' + this.timeText + ' | ' + b + ' (' + m.format('hh:mm A') + ')');

        this.timeText = m.format('hh:mm A');
        this.momentFormattedTimeText = m.format('hh:mm A');

        console.log('TESTING: ' + this.timeText);

        let regexp = new RegExp('\b((1[0-2]|0?[1-9]):([0-5][0-9]) ([AaPp][Mm]))');
        let test = regexp.test(this.timeText);
        console.log('REGEX RESULT: ' + test);

        let n = Number(this.timeText);
        if (n) {
            console.log('THIS IS A NUMBER');
        } else {
            console.log('NOT NOT NOT A NUMBER');
        }
    }
}