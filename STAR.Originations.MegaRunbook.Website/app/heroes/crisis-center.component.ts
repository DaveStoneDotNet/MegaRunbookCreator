
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/heroes/crisis-center.component.html',
    providers: []
})

export class CrisisCenterComponent implements OnInit {

    Title = "Crisis Center";

    constructor() { }

    ngOnInit() {

    }

}