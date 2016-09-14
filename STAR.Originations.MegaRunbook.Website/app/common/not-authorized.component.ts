
import { Component }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Router }          from '@angular/router';
import { ActivatedRoute }  from '@angular/router';

@Component({
    templateUrl: 'app/common/not-authorized.component.html',
    providers:   []
})

export class NotAuthorizedComponent implements OnInit {

    Title = "NOT AUTHORIZED";
    originalUrl: string;

    constructor(private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.data.forEach((data: { crisis: any }) => {
            this.route.queryParams.subscribe(p => this.onQueryParameter(p));
        });
    }

    private onQueryParameter(p) {

        this.originalUrl = p['origin'];
    }

    goBack() {
        if (this.originalUrl) {
            this.router.navigate([this.originalUrl]);
        } else {
            this.router.navigate(['/']);
        }
    }

}