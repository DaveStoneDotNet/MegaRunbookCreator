
import { Injectable }       from '@angular/core';
import { Headers }          from '@angular/http';
import { Http }             from '@angular/http';
import { Response }         from '@angular/http';
import { RequestOptions }   from '@angular/http';

import { IsWorkingService } from '../services/is-working.service';

import { ServiceLink }      from '../entities/service-link.entity';
import { Link }             from '../entities/link.entity';

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

    getServiceLinks(): Promise<ServiceLink[]> {
        this.isWorkingService.startWorking('Working...');
        return this.http.get('api/GetServiceLinks')
            .toPromise()
            .then(response => this.onServiceLinksSuccessful(response))
            .catch(this.handleError);
    }

    private onServiceLinksSuccessful(response: Response) {
        this.isWorkingService.stopWorking();
        let a = response.json();
        return response.json() as ServiceLink[];
    }

    getServiceLink(object: any): Promise<ServiceLink> {

        let body = JSON.stringify(object);

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post('api/GetServiceLink', body, options)
            .toPromise()
            .then(response => this.onServiceLinkSuccessful(response))
            .catch(this.handleError);
    }

    private onServiceLinkSuccessful(response: Response) {
        let a = response.json();
        return response.json() as ServiceLink;
    }

    delete(id: number): Promise<void> {
        let url = '${this.serviceLinkUrl}/${id}';
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    create(name: string): Promise<ServiceLink> {
        return this.http
            .post(this.serviceLinkUrl, JSON.stringify({ name: name }), { headers: this.headers })
            .toPromise()
            .then(res => res.json().data)
            .catch(this.handleError);
    }

    update(hero: ServiceLink): Promise<ServiceLink> {
        const url = '${this.serviceLinkUrl}/${hero.id}';
        return this.http
            .put(url, JSON.stringify(hero), { headers: this.headers })
            .toPromise()
            .then(() => hero)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        this.isWorkingService.stopWorking();
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}