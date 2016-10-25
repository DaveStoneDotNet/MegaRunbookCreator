
import { Injectable }     from '@angular/core';
import { Http }           from '@angular/http';
import { Response }       from '@angular/http';
import { Headers }        from '@angular/http';
import { RequestOptions } from '@angular/http';

import { AppLookups }     from '../entities/app-lookups.entity';
import { HttpService }    from './http.service';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class AppLookupService {

    private lookups: AppLookups;

    constructor(private http: Http) { }

    getAppLookupsPromise(): Promise<Response> {
        return this.http
            .get('api/GetLookups')
            .toPromise();
    }

    getAppLookups(): Promise<AppLookups> {
        return this.http
            .get('api/GetLookups')
            .toPromise()
            .then(response => this.onGetLookupsSuccess(response))
            .catch(this.handleError);
    }

    getLookups(): AppLookups {
        return this.lookups;
    }

    private onGetLookupsSuccess(response: Response) {
        this.lookups = response.json() as AppLookups;
        return this.lookups;
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}