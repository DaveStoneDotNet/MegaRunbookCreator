
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';
       
@Component({
    templateUrl: 'app/admin/admin.component.html',
    providers:  []
})

export class AdminComponent implements OnInit {

    Title = "Admin";

    constructor() { }

    ngOnInit() {

    }

}