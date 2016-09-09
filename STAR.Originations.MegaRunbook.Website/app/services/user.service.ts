
import { Injectable }     from '@angular/core';
import { Http }           from '@angular/http';
import { Response }       from '@angular/http';
import { Headers }        from '@angular/http';
import { RequestOptions } from '@angular/http';

import { UserProfile }    from '../entities/user-profile.entity';
import { HttpService }    from './http.service';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {

    constructor(private http: Http) { }

    getUserProfile(): Promise<UserProfile> {
        return this.http
            .get('api/GetUserProfile')
            .toPromise()
            .then(response => response.json() as UserProfile)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}