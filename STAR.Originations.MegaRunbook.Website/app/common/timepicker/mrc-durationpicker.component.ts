
import { Component }          from '@angular/core';
import { OnInit }             from '@angular/core';
import { OnDestroy }          from '@angular/core';
import { Output }             from '@angular/core';
import { EventEmitter }       from '@angular/core';

import { trigger }            from '@angular/core';
import { state }              from '@angular/core';
import { style }              from '@angular/core';
import { transition }         from '@angular/core';
import { animate }            from '@angular/core';

import { DurationPickerInfo } from './durationpicker.entity';

@Component({
    selector:    'mrc-durationpicker',
    templateUrl: 'app/common/timepicker/mrc-durationpicker.component.html', 
    styleUrls:  ['app/common/timepicker/mrc-timepicker.component.css']
})

export class MrcDurationPickerComponent implements OnInit, OnDestroy {

    @Output() onDurationChanged: EventEmitter<DurationPickerInfo> = new EventEmitter<DurationPickerInfo>();

    messageOpacity: string = '0.3';

    primaryDurations:   DurationPickerInfo[] = [this.getDuration(0, 15), this.getDuration(0, 30), this.getDuration(1, 0), this.getDuration(1.5, 0), this.getDuration(2, 0)];
    secondaryDurations: DurationPickerInfo[] = [this.getDuration(3,  0), this.getDuration(4,  0), this.getDuration(5, 0), this.getDuration(  6, 0), this.getDuration(7, 0)];

    isCollapsed: boolean = true;
    selectedDuration: DurationPickerInfo;

    constructor() {

    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }

    toggleIsCollapsed() {
        this.isCollapsed = !this.isCollapsed;
    }

    onDurationSelected(duration: DurationPickerInfo) {
        this.selectedDuration = duration;
        this.isCollapsed = true;
        this.onDurationChanged.emit(this.selectedDuration);
        this.messageMouseLeave();
    }

    messageMouseEnter() {
        this.messageOpacity = '0.9';
    }

    messageMouseLeave() {
        if (this.isCollapsed) {
            this.messageOpacity = '0.3';
        }
    }

    getDuration(hours: number, minutes: number): DurationPickerInfo {

        let d: DurationPickerInfo = new DurationPickerInfo();
        d.Hours = hours;
        d.Minutes = minutes;

        if (hours > 0) {
            d.DurationText = hours === 1 ? '1 hour' : hours + ' hours';
        } else {
            if (minutes > 0) {
                d.DurationText = minutes === 1 ? '1 min' : minutes + ' mins';
            }
        }
        return d;
    }
}