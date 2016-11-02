
import { Injectable }     from '@angular/core';
import { Http }           from '@angular/http';
import { Response }       from '@angular/http';
import { Headers }        from '@angular/http';
import { RequestOptions } from '@angular/http';

import { UserProfile }    from '../entities/user-profile.entity';
import { Contact }        from '../entities/contact.entity';
import { HttpService }    from './http.service';

import { Observable }     from 'rxjs/Observable';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {

    isAuthenticated: boolean = false;

    private authenticatedUser: UserProfile;

    constructor(private http: Http, private httpService: HttpService) { }

    // ---

    getUserPromise(): Promise<Response> {
        return this.http
            .get('api/GetUserProfile')
            .toPromise();
    }

    // ---

    getUserProfile(): Promise<UserProfile> {
        return this.http
            .get('api/GetUserProfile')
            .toPromise()
            .then(response => this.onGetUserProfileSuccess(response))
            .catch(this.handleError);
    }

    getContacts(): Promise<Contact[]> {
        return this.http
            .get('api/GetContacts')
            .toPromise()
            .then(response => this.onGetContactsSuccess(response))
            .catch(this.handleError);
    }

    // ---

    getAuthenticatedUser(): UserProfile {
        return this.authenticatedUser;
    }

    setAuthenticatedUser(userProfile: UserProfile): void {
        this.authenticatedUser = userProfile;
    }

    getUserIsInRole(roleName: string): boolean {
        let isInRole: boolean = false;
        if (roleName) {
            if (this.authenticatedUser) {
                if (this.authenticatedUser.RoleNames) {
                    isInRole = this.authenticatedUser.RoleNames.filter(r => r === roleName).length > 0;
                }
            }
        }
        return isInRole;
    }

    // ---

    private onGetUserProfileSuccess(response: Response) {
        this.authenticatedUser = response.json() as UserProfile
        this.isAuthenticated = this.authenticatedUser && this.authenticatedUser.IsAuthenticated;
        return this.authenticatedUser;
    }

    private onGetContactsSuccess(response: Response) {
        return response.json() as Contact[];
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

    // ---

    getContactsObservable(request: Contact): Observable<Contact[]> {
        return this.httpService.httpPost({ UserId: request.UserId, DisplayName: request.DisplayName }, 'api/GetContacts')
            .map(this.extractData)
            .catch(this.handleContactsError);
    }

    private extractData(res: Response) {
        return res;
        //let body = res.json();
        //return body.data || {};
    }

    private handleContactsError(error: Response | any) {

        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}