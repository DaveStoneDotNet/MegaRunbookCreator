﻿
import { Injectable }       from '@angular/core';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';

import { Observable }       from 'rxjs/Rx';

import { IsWorkingService } from '../services/is-working.service';

import { HttpService }      from '../services/http.service';

@Injectable()
export class ReleaseService {

    constructor(private http: Http, private httpService: HttpService, private isWorkingService: IsWorkingService) { }

    getRunbookTemplates(): Observable<any> {
        this.isWorkingService.startWorking('Getting Runbok Templates...');
        return this.httpService.httpGet('api/GetRunbookTemplates');
    }

    getRunbookTemplate(id: number): Observable<any> {
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookTemplate');
    }

    getRunbookSteps(): Observable<any> {
        return this.httpService.httpGet('api/GetRunbookSteps');
    }

    getRunbookStep(id: number): Observable<any> {
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookStep');
    }
}
