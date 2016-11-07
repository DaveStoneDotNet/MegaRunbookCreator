
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
import { KeyCodes }       from '../keycodes';

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

    momentTime: any;

    timeText: string = '';
    momentFormattedTimeText: string;

    keyCodes: KeyCodes = new KeyCodes();
    validKeyCodes: Array<number> = new Array();

    constructor() {

    }

    ngOnInit() {
        this.keyCodes = new KeyCodes();
        this.initializeValidKeyCodes();
    }

    ngOnDestroy() {
    }

    toggleIsCollapsed(): void {
        this.isCollapsed = !this.isCollapsed;
    }

    onTimeSelected(time: TimePickerInfo) {
        this.selectedTime = time;
        this.isCollapsed = true;
        this.notify.emit(this.selectedTime);
    }

    getTimePickerInfo(): TimePickerInfo {

        let t = new TimePickerInfo();

        if (this.momentTime) {
            t.MomentValue = this.momentTime;
            t.TimeValue = this.momentTime.toDate();
            t.TimeText = this.momentTime.format('hh:mm A');
            t.Hour = this.momentTime.hour();
            t.Minute = this.momentTime.minute();
            t.Second = 0;
        }

        return t;
    }

    clearSelectedTime(): void {
        this.timeText = '';
        this.momentFormattedTimeText = '';
        this.selectedTime = null;
        this.momentTime = null;
        this.isValid = false;
    }

    timeInputKeyDown(event) {

        let keyCode = event.keyCode;

        if (!this.isValidInputKey(event)) {
            event.preventDefault();
        }
    }

    timeInputKeyUp(event): void {

        let keyCode = event.keyCode;

        if (this.isValidInputKey(event)) {

            let key = '';
            let currentText = event.srcElement.value;

            let currentNumber = +currentText;
            if (isNaN(currentNumber)) {
                console.log('NOT A NUMBER')
            } else {
                console.log('NUMBER')
                if (currentNumber > 24) {

                    // Numbers up to 24 can be parsed by moment.
                    // But if a user beings typing in something like 93 for 9:30, moment returns invalid date.
                    // To fix this, take the first character, e.g. [9], and append a 'colon' after it.

                    let length = currentText.length;
                    let firstDigit = +currentText[0];
                    this.timeText = currentText[0] + ':' + currentText.substring(1, length);
                    currentText = this.timeText;
                    console.log(currentText);
                }
            }

            this.momentTime = moment(currentText, 'h:m A');
            this.setMomentFormattedTimeText();

        } else {
            event.preventDefault();
        }
    }

    timeInputKeyPressed(event): void {
        let currentText = event.srcElement.value;
    }

    timeInputFocused(event): void {

        this.isBlurred = false;
        this.setMomentFormattedTimeText();
    }

    timeInputBlurred(event): void {

        this.isBlurred = true;
        this.momentTime = moment(this.timeText, 'h:m A');
        this.setMomentFormattedTimeText();

        if (this.isValid) {
            this.timeText = this.momentTime.format('hh:mm A');
            this.selectedTime = this.getTimePickerInfo();
        }
    }

    setMomentTime(): void {

        if (this.momentTime) {
            
        }
    }

    setMomentFormattedTimeText(): void {

        if (this.momentTime) {
            this.isValid = this.momentTime.isValid();
            if (this.isValid) {
                this.momentFormattedTimeText = this.momentTime.format('hh:mm A');
            } else {
                if (this.timeText === '') {
                    this.momentFormattedTimeText = 'empty';
                } else {
                    this.momentFormattedTimeText = 'invalid time';
                }
                this.timeText = '';
            }
        }
    }

    initializeValidKeyCodes(): void {

        this.validKeyCodes.push.apply(this.validKeyCodes, this.keyCodes.digits);

        this.validKeyCodes.push(KeyCodes.KEY_A);
        this.validKeyCodes.push(KeyCodes.KEY_P);
        this.validKeyCodes.push(KeyCodes.KEY_M);
        this.validKeyCodes.push(KeyCodes.KEY_SEMICOLON);
        this.validKeyCodes.push(KeyCodes.KEY_SPACE);
    }

    isValidInputKey(event): boolean {

        let isValidInputKey: boolean = false;

        let keyCode = event.keyCode;

        if (this.validKeyCodes.find(k => k === keyCode)) {
            return true;
        }

        if (this.keyCodes.isEditKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isNavKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isModifierKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isFunctionKey(keyCode)) {
            return true;
        }

        if (event.shiftKey) {
            if (keyCode === KeyCodes.KEY_SEMICOLON) {
                return true;
            }
        }

        return isValidInputKey;
    }

    isValidTimeKey(event): boolean {

        let isValidTimeKey: boolean = false;

        let keyCode = event.keyCode;

        if (this.validKeyCodes.find(k => k === keyCode)) {
            return true;
        }

        if (this.keyCodes.isEditKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isNavKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isModifierKey(keyCode)) {
            return true;
        }

        if (this.keyCodes.isFunctionKey(keyCode)) {
            return true;
        }

        if (event.shiftKey) {
            if (keyCode === KeyCodes.KEY_SEMICOLON) {
                return true;
            }
        }

        return isValidTimeKey;
    }
}