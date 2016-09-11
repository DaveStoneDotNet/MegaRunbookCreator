
import { Injectable }     from '@angular/core';
import { Headers }        from '@angular/http';
import { Http }           from '@angular/http';
import { Response }       from '@angular/http';
import { RequestOptions } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Hero } from './hero';

@Injectable()
export class HeroService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private heroesUrl = 'api';
    private jsonHeaders: Headers;

    constructor(private http: Http) {

        this.jsonHeaders = new Headers();
        this.jsonHeaders.append('Content-Type', 'application/json');
        this.jsonHeaders.append('Accept', 'q=0.8;application/json;q=0.9');
    }

    getHeroes(): Promise<Hero[]> {
        return this.http.get('api/GetHeroes')
            .toPromise()
            .then(response => this.onHeroesSuccessful(response))
            .catch(this.handleError);
    }

    private onHeroesSuccessful(response: Response) {
        let a = response.json();
        return response.json() as Hero[];
    }

    getHero(object: any): Promise<Hero> {

        let body = JSON.stringify(object);

        let options = new RequestOptions({ headers: this.jsonHeaders });

        return this.http.post('api/GetHero', body, options)
            .toPromise()
            .then(response => this.onHeroSuccessful(response))
            .catch(this.handleError);
    }

    private onHeroSuccessful(response: Response) {
        let a = response.json();
        return response.json() as Hero;
    }

    delete(id: number): Promise<void> {
        let url = '${this.heroesUrl}/${id}';
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }

    create(name: string): Promise<Hero> {
        return this.http
            .post(this.heroesUrl, JSON.stringify({ name: name }), { headers: this.headers })
            .toPromise()
            .then(res => res.json().data)
            .catch(this.handleError);
    }

    update(hero: Hero): Promise<Hero> {
        const url = '${this.heroesUrl}/${hero.id}';
        return this.http
            .put(url, JSON.stringify(hero), { headers: this.headers })
            .toPromise()
            .then(() => hero)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
}