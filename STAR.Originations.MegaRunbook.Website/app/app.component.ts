
import { Component }                 from '@angular/core';
import { OnInit }                    from '@angular/core';
import { OnDestroy }                 from '@angular/core';

import { AppService }                from './services/app.service';
import { IsWorkingService }          from './services/is-working.service';
import { IsWorkingComponent }        from './common/isworking/is-working.component';
import { IsWorkingSpinnerComponent } from './common/isworking/is-working-spinner.component';

import { AppInitialization }         from './entities/app-initialization.entity';
import { UserProfile }               from './entities/user-profile.entity';

@Component({
    selector: 'mrc-app',
    templateUrl: 'app/app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

    today = new Date();

    isWorking = false;
    workingMessage: string;
    workingMessageSubscription: any;
    isWorkingSubscription: any;
    isAppInitialized = false;
    appInitialization: AppInitialization;
    authenticatedUser: UserProfile;

    constructor(private isWorkingService: IsWorkingService, private appService: AppService) {
        this.appService.onAppInitializationChanged.subscribe((event: AppInitialization) => {
            this.appInitialization = event;
            this.isAppInitialized = this.appInitialization.IsInitialized;
            this.authenticatedUser = this.appService.authenticatedUser;
        });
    }

    ngOnInit() {
        this.workingMessageSubscription = this.isWorkingService.isWorkingEvent
            .subscribe(event => this.onWorking(event));
        this.isWorkingSubscription = this.isWorkingService.isWorking.subscribe(value => this.isWorking = value);
        this.appService.initialize();
    }

    ngOnDestroy() {
        this.workingMessageSubscription.unsubscribe();
        this.isWorkingSubscription.unsubscribe();
    }

    onFooterClicked() {
        this.isWorking = false;
    }

    private onWorking(message) {
        this.workingMessage = message;
    }
}
