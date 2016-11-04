
import { Component }    from '@angular/core';
import { OnInit }       from '@angular/core';
import { OnDestroy }    from '@angular/core';
import { Output }       from '@angular/core';
import { EventEmitter } from '@angular/core';

import { trigger }      from '@angular/core';
import { state }        from '@angular/core';
import { style }        from '@angular/core';
import { transition }   from '@angular/core';
import { animate }      from '@angular/core';

@Component({
    selector:    'mrc-durationpicker',
    templateUrl: 'app/common/timepicker/mrc-durationpicker.component.html', 
    styleUrls:  ['app/common/timepicker/mrc-timepicker.component.css']
})

export class MrcDurationPickerComponent implements OnInit, OnDestroy {

    @Output() onDurationChanged: EventEmitter<string> = new EventEmitter<string>();

    primaryDurations: string[] = ['15 minutes', '30 minutes', '1 hour', '1.5 hours', '2 hours'];
    secondaryDurations: string[] = ['3 hours', '4 hours', '5 hours', '6 hours', '7 hours'];

    isCollapsed: boolean = true;
    selectedDuration: string;

    constructor() {

    }

    ngOnInit() {
    }

    ngOnDestroy() {
    }

    toggleIsCollapsed() {
        this.isCollapsed = !this.isCollapsed;
    }

    onDurationSelected(duration: string) {
        this.selectedDuration = duration;
        this.isCollapsed = true;
        this.onDurationChanged.emit(this.selectedDuration);
    }
}