
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { OnDestroy }      from '@angular/core';
import { Router }         from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { Crisis }         from './crisis.service';
import { CrisisService }  from './crisis.service';

import { Subscription }   from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/crisis-center/crisis-list.component.html',
})

export class CrisisListComponent implements OnInit, OnDestroy {

    Title = "Crisis List";
    criseses: Crisis[];

    private selectedId: number;
    private sub: Subscription;

    constructor(private service: CrisisService, private route: ActivatedRoute, private router: Router) {
        
    }

    isSelected(crisis: Crisis) {
        return crisis.Id === this.selectedId;
    }

    ngOnInit() {
        this.sub = this.route
            .params
            .subscribe(params => {
                this.selectedId = +params['id'];
                this.service.getCrisises()
                    .then(crises => this.criseses = crises);
            });
    }

    ngOnDestroy() {
        if (this.sub) {
            this.sub.unsubscribe();
        }
    }

    onSelect(crisis: Crisis) {

        // Navigate with Absolute link
        this.router.navigate(['/crisis-center', crisis.Id]);
    }

    private onCrisisSuccessful(response: Crisis[]) {
        this.criseses = response;
    }
}