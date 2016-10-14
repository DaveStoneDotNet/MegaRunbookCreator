import { Injectable } from '@angular/core';
import { Http }       from '@angular/http';
import { Response }   from '@angular/http';

import { Car }        from './car';

@Injectable()
export class CarService {

    constructor(private http: Http) { }

    getCarsSmall() {
        return this.http.get('json/cars-small.json')
            .toPromise()
            .then(res => <Car[]>res.json().data)
            .then(data => { return data; });
    }

    getCarsMedium() {
        return this.http.get('json/cars-medium.json')
            .toPromise()
            .then(res => <Car[]>res.json().data)
            .then(data => { return data; });
    }

    getCarsLarge() {
        return this.http.get('json/cars-large.json')
            .toPromise()
            .then(res => <Car[]>res.json().data)
            .then(data => { return data; });
    }
}