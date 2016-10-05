
import { Injectable }       from '@angular/core';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';
import { RequestOptions }   from '@angular/http';

import { IsWorkingService } from '../services/is-working.service';

import { HttpService }      from '../services/http.service';

import { ApplicationLink }  from '../entities/application-link.entity';
import { EnvironmentLink }  from '../entities/environment-link.entity';

import { Observable }  from 'rxjs/Rx';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class LinkService {

    constructor(private http: Http, private httpService: HttpService, private isWorkingService: IsWorkingService) { }

    getApplicationLinks(): Observable<any> {
        this.isWorkingService.startWorking('Getting Applications...');
        return this.httpService.httpGet('api/GetApplicationLinks');
    }
}