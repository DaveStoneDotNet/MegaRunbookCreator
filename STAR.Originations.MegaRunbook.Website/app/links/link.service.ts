
import { Injectable }       from '@angular/core';
import { Headers }          from '@angular/http';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';
import { RequestOptions }   from '@angular/http';

import { IsWorkingService } from '../services/is-working.service';

import { ApplicationLink }  from '../entities/application-link.entity';
import { EnvironmentLink }  from '../entities/environment-link.entity';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class LinkService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private serviceLinkUrl = 'api';
    private jsonHeaders: Headers;

    constructor(private http: Http, private isWorkingService: IsWorkingService) {

        this.jsonHeaders = new Headers();
        this.jsonHeaders.append('Content-Type', 'application/json');
        this.jsonHeaders.append('Accept', 'q=0.8;application/json;q=0.9');
    }

    getApplicationLinks(): Promise<ApplicationLink[]> {
        this.isWorkingService.startWorking('Working...');
        return this.http.get('api/GetApplicationLinks')
            .toPromise()
            .then(response => this.onServiceLinksSuccessful(response))
            .catch(this.handleError);
    }

    private onServiceLinksSuccessful(response: Response) {
        this.isWorkingService.stopWorking();
        let a = response.json();
        return response.json() as ApplicationLink[];
    }

    getServiceLink(object: any): Promise<ApplicationLink> {

        let body = JSON.stringify(object);

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post('api/GetServiceLink', body, options)
            .toPromise()
            .then(response => this.onServiceLinkSuccessful(response))
            .catch(this.handleError);
    }

    private onServiceLinkSuccessful(response: Response) {
        let a = response.json();
        return response.json() as ApplicationLink;
    }

    delete(id: number): Promise<void> {
        let url = '${this.serviceLinkUrl}/${id}';
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    create(name: string): Promise<ApplicationLink> {
        return this.http
            .post(this.serviceLinkUrl, JSON.stringify({ name: name }), { headers: this.headers })
            .toPromise()
            .then(res => res.json().data)
            .catch(this.handleError);
    }

    update(applicationLink: ApplicationLink): Promise<ApplicationLink> {
        const url = '${this.serviceLinkUrl}/${applicationLink.id}';
        return this.http
            .put(url, JSON.stringify(applicationLink), { headers: this.headers })
            .toPromise()
            .then(() => applicationLink)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        this.isWorkingService.stopWorking();
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}