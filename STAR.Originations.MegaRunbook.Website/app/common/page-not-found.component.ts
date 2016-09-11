
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/common/page-not-found.component.html',
    providers: []
})

export class PageNotFoundComponent implements OnInit {

    Title = "Page Not Found";

    constructor() { }

    ngOnInit() {

    }

}