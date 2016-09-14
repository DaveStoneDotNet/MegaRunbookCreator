
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

    isAuthenticated: boolean = false;

    private authenticatedUser: UserProfile;

    constructor(private http: Http) { }

    getUserProfile(): Promise<UserProfile> {
        return this.http
            .get('api/GetUserProfile')
            .toPromise()
            .then(response => this.onGetUserProfileSuccess(response))
            .catch(this.handleError);
    }

    getAuthenticatedUser(): UserProfile {
        return this.authenticatedUser;
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

    private onGetUserProfileSuccess(response: Response) {
        this.authenticatedUser = response.json() as UserProfile
        this.isAuthenticated = this.authenticatedUser && this.authenticatedUser.IsAuthenticated;
        return this.authenticatedUser;
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}