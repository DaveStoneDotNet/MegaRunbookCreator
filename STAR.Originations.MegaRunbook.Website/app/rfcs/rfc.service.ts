
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

    insertRfc(rfc: RFC): Observable<any> {
        return this.httpService.httpPost(rfc, 'api/InsertRfc');
    }
}
