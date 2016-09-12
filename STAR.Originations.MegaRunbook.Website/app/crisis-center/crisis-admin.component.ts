﻿
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable }     from 'rxjs/Observable';

import 'rxjs/add/operator/map';

@Component({
    templateUrl: 'app/crisis-center/crisis-admin.component.html',
})
export class CrisisAdminComponent implements OnInit {

    Title = "Crisis Center Administration";

    sessionId: Observable<string>;
    token: Observable<string>;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {

        // Capture the session ID if available
        this.sessionId = this.route
            .queryParams
            .map(params => params['session_id'] || 'None');

        // Capture the fragment if available
        this.token = this.route
            .fragment
            .map(fragment => fragment || 'None');
    }
}
