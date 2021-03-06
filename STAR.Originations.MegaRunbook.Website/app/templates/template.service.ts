﻿
import { Injectable }       from '@angular/core';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';

import { Observable }       from 'rxjs/Rx';

import { IsWorkingService } from '../services/is-working.service';

import { HttpService }      from '../services/http.service';

@Injectable()
export class TemplateService {

    constructor(private http: Http, private httpService: HttpService, private isWorkingService: IsWorkingService) { }

    getRunbookTemplates(): Observable<any> {
        this.isWorkingService.startWorking('Getting Runbook Templates...');
        return this.httpService.httpGet('api/GetRunbookTemplates');
    }

    getRunbookTemplate(id: number): Observable<any> {
        this.isWorkingService.startWorking('Getting Runbook Template...');
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookTemplate');
    }

    getRunbookSteps(): Observable<any> {
        this.isWorkingService.startWorking('Getting Runbook Steps...');
        return this.httpService.httpGet('api/GetRunbookSteps');
    }

    getRunbookStep(id: number): Observable<any> {
        this.isWorkingService.startWorking('Getting Runbook Step...');
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookStep');
    }
}
