
import { Injectable }       from '@angular/core';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';

import { RFC }              from '../entities/rfc.entity';

import { IsWorkingService } from '../services/is-working.service';
import { HttpService }      from '../services/http.service';

import { Observable }       from 'rxjs/Rx';

@Injectable()
export class RfcService {

    constructor(private http: Http, private httpService: HttpService, private isWorkingService: IsWorkingService) { }

    getRfcs(rfc: RFC): Observable<any> {
        this.isWorkingService.startWorking('Getting RFCs...');
        return this.httpService.httpPost(rfc, 'api/GetRfcs');
    }

    getRfc(id: number): Observable<any> {
        this.isWorkingService.startWorking('Getting RFC...');
        return this.httpService.httpPost({ id: id }, 'api/GetRfc');
    }

    insertRfc(rfc: RFC): Observable<any> {
        this.isWorkingService.startWorking('Saving RFC...');
        return this.httpService.httpPost(rfc, 'api/InsertRfc');
    }
}
