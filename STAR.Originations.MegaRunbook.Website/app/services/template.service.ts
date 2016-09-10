
import { Injectable }  from '@angular/core';
import { Http }        from '@angular/http';
import { Response }    from '@angular/http';

import { Observable }  from 'rxjs/Rx';

import { HttpService } from './http.service';

@Injectable()
export class TemplateService {
    constructor(private http: Http, private httpService: HttpService) { }

    searchTemplates(): Observable<any> {
        return this.httpService.httpGet('api/GetRunbookTemplates');
    }

    getRunbookTemplate(id: Number): Observable<any> {
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookTemplate');
    }

    getRunbookSteps(): Observable<any> {
        return this.httpService.httpGet('api/GetRunbookSteps');
    }

    getRunbookStep(id: Number): Observable<any> {
        return this.httpService.httpPost({ id: id }, 'api/GetRunbookStep');
    }
}
