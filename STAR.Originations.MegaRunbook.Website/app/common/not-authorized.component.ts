
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { OnDestroy }       from '@angular/core';
import { Router }          from '@angular/router';
import { ActivatedRoute }  from '@angular/router';

import { Subscription }    from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/common/not-authorized.component.html',
    providers:   []
})

export class NotAuthorizedComponent implements OnInit, OnDestroy {

    Title = "NOT AUTHORIZED";
    originalUrl: string;

    private subscription: Subscription;

    constructor(private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {

        this.subscription = this.route
            .queryParams
            .subscribe(p => this.onQueryParameterEmitted(p));
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    goBack() {
        if (this.originalUrl) {
            this.router.navigate([this.originalUrl]);
        } else {
            this.router.navigate(['/']);
        }
    }

    private onQueryParameterEmitted(p) {

        this.originalUrl = p['origin'];
    }
}