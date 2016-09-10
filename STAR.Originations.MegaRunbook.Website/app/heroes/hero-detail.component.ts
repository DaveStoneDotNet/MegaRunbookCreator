
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/heroes/hero-detail.component.html',
    providers: []
})

export class HeroDetailComponent implements OnInit {

    Title = "Hero Detail";

    constructor() { }

    ngOnInit() {

    }

}