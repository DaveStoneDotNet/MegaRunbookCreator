
import { Injectable }        from '@angular/core';
import { EventEmitter }      from '@angular/core';
import { Http }              from '@angular/http';
import { Response }          from '@angular/http';
import { Headers }           from '@angular/http';
import { RequestOptions }    from '@angular/http';

import { HttpService }       from './http.service';
import { UserService }       from './user.service';

import { AppInitialization } from '../entities/app-initialization.entity';
import { AppLookups }        from '../entities/app-lookups.entity';
import { UserProfile }       from '../entities/user-profile.entity';
import { Message }           from '../entities/message.entity';

import { AppLookupService }  from './app-lookup.service';
import { MessageService }    from './message.service';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class AppService {

    onAppInitializationChanged = new EventEmitter<AppInitialization>();

    appIsInitialized: boolean = false;
    isAuthenticated: boolean = false;

    authenticatedUser: UserProfile;
    lookups: AppLookups;

    constructor(private http: Http, private userService: UserService, private appLookupService: AppLookupService, private messageService: MessageService) { }

    initialize(): void {
        setTimeout(() => { this.getUserProfile(); }, 3000);
        this.getLookups();
    }

    getUserProfile(): Promise<UserProfile> {
        return this.userService.getUserProfile()
            .then(response => this.onGetUserProfileSuccess(response))
            .catch(this.handleError);
    }

    getLookups(): Promise<AppLookups> {
        return this.appLookupService.getAppLookups()
            .then(response => this.onGetLookupsSuccess(response))
            .catch(this.handleError);
    }

    private onGetUserProfileSuccess(userProfile: UserProfile) {
        this.authenticatedUser = userProfile
        this.isAuthenticated = this.authenticatedUser && this.authenticatedUser.IsAuthenticated;
        this.isAppInitialized(`${this.authenticatedUser.UserDisplayName} Authenticated`);
        this.userService.setAuthenticatedUser(userProfile);
        return this.authenticatedUser;
    }

    private onGetLookupsSuccess(lookups: AppLookups) {
        this.lookups = lookups;
        this.isAppInitialized('Lookups');
        return this.lookups;
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

    private isAppInitialized(message: string) {
        if (this.isAuthenticated) {
            if (this.lookups) {
                this.appIsInitialized = true;
                let message = new Message('READY');
                this.messageService.sendMessage(message);
            }
        }
        let appInitialization = new AppInitialization(this.appIsInitialized, message);
        this.onAppInitializationChanged.emit(appInitialization);
    }
}