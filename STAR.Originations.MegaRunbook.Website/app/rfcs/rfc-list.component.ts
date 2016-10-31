
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';

@Component({
    templateUrl: 'app/rfcs/rfc-list.component.html',
    providers:   []
})

export class RfcListComponent implements OnInit {

    Title = "RFC's";

    constructor(private router: Router) { }

    ngOnInit() {

    }

    addRfcClick() {
        this.router.navigate(['add-rfc']);
    }

    editRfcClick() {
        this.router.navigate(['edit-rfc']);
    }
}