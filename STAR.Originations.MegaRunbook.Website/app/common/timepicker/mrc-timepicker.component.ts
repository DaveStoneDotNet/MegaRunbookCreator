
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

import * as moment      from 'moment';

@Component({
    selector:    'mrc-timepicker',
    templateUrl: 'app/common/timepicker/mrc-timepicker.component.html', 
    styleUrls:   ['app/common/timepicker/mrc-timepicker.component.css']
})

export class MrcTimePickerComponent implements OnInit, OnDestroy {

    @Output() notify: EventEmitter<TimePickerInfo> = new EventEmitter<TimePickerInfo>();

    amSet_A: TimePickerInfo[] = [
        this.getTime(0, 0),
        this.getTime(1, 0),
        this.getTime(2, 0),
        this.getTime(3, 0),
        this.getTime(4, 0),
        this.getTime(5, 0)
    ];

    amSet_B: TimePickerInfo[] = [
        this.getTime( 6, 0),
        this.getTime( 7, 0),
        this.getTime( 8, 0),
        this.getTime( 9, 0),
        this.getTime(10, 0),
        this.getTime(11, 0)
    ];

    pmSet_A: TimePickerInfo[] = [
        this.getTime(12, 0),
        this.getTime(13, 0),
        this.getTime(14, 0),
        this.getTime(15, 0),
        this.getTime(16, 0),
        this.getTime(17, 0)
    ];

    pmSet_B: TimePickerInfo[] = [
        this.getTime(18, 0),
        this.getTime(19, 0),
        this.getTime(20, 0),
        this.getTime(21, 0),
        this.getTime(22, 0),
        this.getTime(23, 0)
    ];

    isCollapsed: boolean = true;
    selectedTime: TimePickerInfo;

    today: string = moment().format('D MMM YYYY');
    tomorrow: Date = moment().toDate();

    constructor() {

    }

    ngOnInit() {
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
        t.TimeValue = m.toDate();
        t.TimeText = m.format('hh:mm A');
        t.Hour = hours;
        t.Minute = minutes;
        t.Second = 0;
        return t;
    }
}