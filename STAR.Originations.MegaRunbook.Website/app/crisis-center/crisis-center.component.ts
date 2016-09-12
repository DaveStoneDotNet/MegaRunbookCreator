
import { Component } from '@angular/core';
import { Router }    from '@angular/router';

@Component({
    templateUrl: 'app/crisis-center/crisis-center.component.html',
})

export class CrisisCenterComponent {
    
    Title = "Crisis Center";

    constructor(private router: Router) {

    }

    gotoAdmin() {

        this.router.navigate(['/crisis-center/crisis-admin']);
    }
}