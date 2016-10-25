
import { Component }         from '@angular/core';
import { OnInit }            from '@angular/core';

import { AppService }        from '../services/app.service';

import { UserProfile }       from '../entities/user-profile.entity';
import { AppInitialization } from '../entities/app-initialization.entity';

@Component({
    selector:    'mrc-home',
    templateUrl: 'app/home/home.component.html' 
})

export class HomeComponent implements OnInit {

    today: Date;
    userProfile: UserProfile;

    constructor(private appService: AppService) {

    }

    ngOnInit() {
        this.today = new Date();

        this.appService.onAppInitializationChanged.subscribe((event: AppInitialization) => {
            this.userProfile = this.appService.authenticatedUser;
        });

    }
}