
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/heroes/hero-list.component.html',
    providers: []
})

export class HeroListComponent implements OnInit {

    Title = "Hero List";

    constructor() { }

    ngOnInit() {

    }

}