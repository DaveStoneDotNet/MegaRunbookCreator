
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/rfcs/rfc-edit.component.html',
    providers: []
})

export class RfcEditComponent implements OnInit {

    Title = "Edit RFC";

    constructor(private router: Router) { }

    ngOnInit() {

    }

    listRfcsClick() {
        this.router.navigate(['rfcs']);
    }
}