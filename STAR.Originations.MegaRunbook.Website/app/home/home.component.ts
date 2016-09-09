
import { Component }   from '@angular/core';
import { OnInit }      from '@angular/core';

import { UserService } from '../services/user.service';
import { UserProfile } from '../entities/user-profile.entity';

@Component({
    selector:    'mrc-home',
    templateUrl: 'app/home/home.component.html', 
    providers:   [UserService]
})

export class HomeComponent implements OnInit {

    today: Date;
    userProfile: UserProfile;

    constructor(private userService: UserService) {
        
    }

    ngOnInit() {
        this.userService.getUserProfile().then(userProfile => this.onUserProfileSucceeded(userProfile));
        this.today = new Date();
    }

    onUserProfileSucceeded(userProfile: UserProfile) {
        this.userProfile = userProfile;
    }
}