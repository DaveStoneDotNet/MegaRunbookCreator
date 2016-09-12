
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { Router }         from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { Crisis }         from './crisis.service';
import { DialogService }  from '../services/dialog.service';

import { Observable }     from 'rxjs/Observable';

@Component({
    templateUrl: './crisis-detail.component.html',
})

export class CrisisDetailComponent implements OnInit {

    crisis: Crisis;
    editName: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        public dialogService: DialogService
    ) { }

    ngOnInit() {
        this.route.data.forEach((data: { crisis: Crisis }) => {
            this.editName = data.crisis.Name;
            this.crisis = data.crisis;
        });
    }

    cancel() {
        this.gotoCrises();
    }

    save() {
        this.crisis.Name = this.editName;
        this.gotoCrises();
    }

    canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {

        // Allow synchronous navigation (`true`) if no crisis or the crisis is unchanged
        if (!this.crisis || this.crisis.Name === this.editName) {
            return true;
        }

        // Otherwise ask the user with the dialog service and return its
        // promise which resolves to true or false when the user decides
        return this.dialogService.confirm('Discard changes?');
    }

    gotoCrises() {

        let crisisId = this.crisis ? this.crisis.Id : null;

        // Pass along the hero id if available
        // so that the CrisisListComponent can select that hero.
        // Add a totally useless `foo` parameter for kicks.
        // Absolute link
        this.router.navigate(['/crisis-center', { id: crisisId, foo: 'foo' }]);
    }
}