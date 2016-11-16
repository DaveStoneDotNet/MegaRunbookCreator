
import { Component }            from '@angular/core';
import { OnInit }               from '@angular/core';
import { OnDestroy }            from '@angular/core';
import { Input }                from '@angular/core';
import { Output }               from '@angular/core';
import { EventEmitter }         from '@angular/core';

import { trigger }              from '@angular/core';
import { state }                from '@angular/core';
import { style }                from '@angular/core';
import { transition }           from '@angular/core';
import { animate }              from '@angular/core';

import { forwardRef }           from '@angular/core';
import { NG_VALUE_ACCESSOR }    from '@angular/forms';
import { ControlValueAccessor } from '@angular/forms';

import { TimePickerInfo }       from './timepicker.entity';
import { KeyCodes }             from '../keycodes';

import * as moment              from 'moment';

const noop = () => { };

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => MrcTimeInputComponent),
    multi: true
};

@Component({
    selector:    'mrc-time-input',
    templateUrl: 'app/common/timepicker/mrc-time-input.component.html',
    styleUrls:  ['app/common/timepicker/mrc-time-input.component.css'], 
    providers:  [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})

export class MrcTimeInputComponent implements OnInit, OnDestroy, ControlValueAccessor  {

    @Input() selectedTime: TimePickerInfo = new TimePickerInfo();
    @Output() timeInputChanged: EventEmitter<TimePickerInfo> = new EventEmitter<TimePickerInfo>();

    originalTime: TimePickerInfo = new TimePickerInfo();

    isCollapsed: boolean = true;
    isBlurred: boolean = true;
    isValid: boolean = false;

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
        this.timeInputChanged.emit(this.selectedTime);
    }

    getTimePickerInfo(): TimePickerInfo {

        let t = new TimePickerInfo();

        if (this.selectedTime.MomentValue) {
            t.MomentValue = this.selectedTime.MomentValue;
            t.TimeValue = this.selectedTime.MomentValue.toDate();
            t.TimeText = this.selectedTime.MomentValue.format('hh:mm A');
            t.Hour = this.selectedTime.MomentValue.hour();
            t.Minute = this.selectedTime.MomentValue.minute();
            t.Second = 0;
        }

        return t;
    }

    clearSelectedTime(): void {
        this.selectedTime = new TimePickerInfo();;
        this.momentFormattedTimeText = '';
        this.selectedTime.MomentValue = null;
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
                // NOT A NUMBER
            } else {
                if (currentNumber > 24) {

                    // Numbers up to 24 can be parsed by moment.
                    // But if a user beings typing in something like 93 for 9:30, moment returns invalid date.
                    // To fix this, take the first character, e.g. [9], and append a 'colon' after it.

                    let length = currentText.length;
                    let firstDigit = +currentText[0];
                    this.selectedTime.TimeText = currentText[0] + ':' + currentText.substring(1, length);
                    currentText = this.selectedTime.TimeText;
                }
            }

            this.setSelectedTimeOnKeyUp(currentText);

        } else {
            event.preventDefault();
        }
    }

    timeInputKeyPressed(event): void {
        let currentText = event.srcElement.value;
    }

    timeInputFocused(event): void {

        this.isBlurred = false;

        this.originalTime.Hour = this.selectedTime.Hour;
        this.originalTime.Minute = this.selectedTime.Minute;

        this.selectedTime = this.parseTimeText(this.selectedTime.TimeText);
        this.setMomentFormattedTimeText();
    }

    timeInputBlurred(event): void {

        this.isBlurred = true;
        this.selectedTime.MomentValue = moment(this.selectedTime.TimeText, 'h:m A');
        this.setMomentFormattedTimeText();

        if (this.isValid) {
            this.selectedTime.TimeText = this.selectedTime.MomentValue.format('hh:mm A');
            this.selectedTime = this.getTimePickerInfo();

            if (this.originalTime.Hour != this.selectedTime.Hour || this.originalTime.Minute != this.selectedTime.Minute) {
                this.onTimeSelected(this.selectedTime);
            }
        }
        this.onTouchedCallback();
    }

    setMomentFormattedTimeText(): void {

        if (this.hasTime(this.selectedTime)) {
            if (this.selectedTime.MomentValue) {
                this.isValid = this.selectedTime.MomentValue.isValid();
                if (this.isValid) {
                    this.momentFormattedTimeText = this.selectedTime.MomentValue.format('hh:mm A');
                } else {
                    if (this.selectedTime.TimeText === '') {
                        this.momentFormattedTimeText = 'empty';
                    } else {
                        this.momentFormattedTimeText = 'invalid time';
                    }
                    this.selectedTime.TimeText = '';
                }
            }
        } else {
            this.momentFormattedTimeText = 'empty';
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

    parseTimeText(timeText: string): TimePickerInfo {
        let t = new TimePickerInfo();
        t.TimeText = timeText;
        t.MomentValue = moment(timeText, 'hh:mm A');
        t.TimeValue = t.MomentValue.toDate();
        t.Hour = t.MomentValue.hour();
        t.Minute = t.MomentValue.minute();
        t.Second = 0;
        this.selectedTime.MomentValue = t.MomentValue;
       return t;
    }

    hasTime(timePickerInfo: TimePickerInfo): boolean {

        let b = false;

        if (timePickerInfo) {
            if (timePickerInfo.Hour !== undefined && timePickerInfo.Minute !== undefined) {
                if (!isNaN(timePickerInfo.Hour) && !isNaN(timePickerInfo.Minute)) {
                    b = true;
                }
            }
        }

        return b;
    }

    setSelectedTimeOnKeyUp(timeText: string): void {

        this.selectedTime.MomentValue = moment(timeText, 'h:m A');

        this.selectedTime.TimeValue = this.selectedTime.MomentValue.toDate();
        this.selectedTime.Hour = this.selectedTime.MomentValue.hour();
        this.selectedTime.Minute = this.selectedTime.MomentValue.minute();
        this.selectedTime.Second = 0;

        this.setMomentFormattedTimeText();
    }

    // ----------------------------------------------------------------------------------------------
    // ControlValueAccessor interface
    // ----------------------------------------------------------------------------------------------

    // The internal data model

    // Placeholders for the callbacks which are later provided by the Control Value Accessor
    private onTouchedCallback: () => void = noop;
    private onChangeCallback: (_: any) => void = noop;

    //get accessor
    get mrcTimeInput(): any {
        return this.selectedTime.TimeText;
    };

    //set accessor including call the onchange callback
    set mrcTimeInput(timeText: any) {
        if (timeText !== this.selectedTime.TimeText) {
            this.selectedTime.TimeText = timeText;
            this.onChangeCallback(timeText);
        }
    }

    // Set touched on blur
    onBlur() {
        this.onTouchedCallback();
    }

    // The writeValue function allows you to update your internal model with incoming values, 
    // for example if you use ngModel to bind your control to data.
    writeValue(value: any) {
        if (value !== this.selectedTime.TimeText) {
            this.selectedTime.TimeText = value;
        }
    }

    // ---
    // NOTE: Both the functions below are later provided by Angular 2 itself. But we need to register 
    // dummy functions to be to code and transpile it without errors.
    // ---

    // From ControlValueAccessor interface
    // The registerOnChange accepts a callback function which you can call when changes happen 
    // so that you can notify the outside world that the data model has changed. Note that you 
    // call it with the changed data model value.
    registerOnChange(fn: any) {
        console.log('ON CHANGED');
        this.onChangeCallback = fn;
    }

    // From ControlValueAccessor interface
    // The registerOnTouched function accepts a callback function which you can call when you 
    // want to set your control to touched. This is then managed by Angular 2 by adding the 
    // correct touched state and classes to the actual element tag in the DOM.
    registerOnTouched(fn: any) {
        console.log('ON TOUCHED');
        this.onTouchedCallback = fn;
    }

    emitValue(event) {
        console.log('EMIT VALUE');
    }

}