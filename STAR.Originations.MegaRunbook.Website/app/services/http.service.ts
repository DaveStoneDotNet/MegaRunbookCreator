
import { Injectable }       from '@angular/core';
import { EventEmitter }     from '@angular/core';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';
import { Headers }          from '@angular/http';
import { RequestOptions }   from '@angular/http';

import { Observable }       from 'rxjs/Rx';

import { BlockUIService }   from './blockui.service';
import { IsWorkingService } from '../services/is-working.service';

@Injectable()
export class HttpService {

    private jsonHeaders: Headers;

    constructor(private http: Http, private blockUIService: BlockUIService, private isWorkingService: IsWorkingService) {

        this.jsonHeaders = new Headers();
        this.jsonHeaders.append('Content-Type', 'application/json');
        this.jsonHeaders.append('Accept', 'q=0.8;application/json;q=0.9');
    }

    httpPost(object: any, url: string): Observable<any> {

        this.isWorkingService.startWorking('Working...');
        this.blockUIService.startBlock();
     
        let body = JSON.stringify(object);

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post(url, body, options)
                   .map((response) => this.parseResponse(response, this.blockUIService, true))
                   .catch((err) => this.handleError(err, this.blockUIService, true));
    }

    httpPostWithNoBlock(object: any, url: string): Observable<any> {

        this.isWorkingService.startWorking('Working...');
        let body = JSON.stringify(object);

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post(url, body, options)
                   .map((response) => this.parseResponse(response, this.blockUIService, false))
                   .catch((err) => this.handleError(err, this.blockUIService, false));
    }

    httpGet(url: string): Observable<any> {

        this.isWorkingService.startWorking('Working...');
        this.blockUIService.startBlock();

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post(url, options)
            .map((response) => this.parseResponse(response, this.blockUIService, true))
            .catch((err) => this.handleError(err, this.blockUIService, true));
    }

    httpGetWithNoBlock(url: string): Observable<any> {

        this.isWorkingService.startWorking('Working...');
        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.get(url, options)
            .map((response) => this.parseResponse(response, this.blockUIService, false))
            .catch((err) => this.handleError(err, this.blockUIService, false));
    }

    private handleError(error: any, blockUIService: BlockUIService, blocking: Boolean) {

        this.isWorkingService.stopWorking();
        let body = error.json();

        if (blocking) {
            blockUIService.stopBlock();
        }
     
        return Observable.throw(body);
    }

    private parseResponse(response: Response, blockUIService: BlockUIService, blocking: Boolean) {

        this.isWorkingService.stopWorking();
        if (blocking) {
            blockUIService.stopBlock();
        }
     
        let body = response.json();

        return body;
    }
}