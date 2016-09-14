
import { Injectable }             from '@angular/core';
import { CanActivate }            from '@angular/router';
import { Router }                 from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { RouterStateSnapshot }    from '@angular/router';
import { NavigationExtras }       from '@angular/router';

import { AuthService }            from './auth.service';
import { UserService }            from './user.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        let routeCanNavigate = false;

        // Going away from the Developer Guide here since this site is using Windows Auth.

        let isAuthenticated = false;
        let authenticatedUser = this.userService.getAuthenticatedUser();
        if (authenticatedUser) {
            isAuthenticated = authenticatedUser.IsAuthenticated;
        }

        if (isAuthenticated) {
            routeCanNavigate = true;
        } else {

            // Set our navigation extras object that contains our global query params and fragment
            let navigationExtras: NavigationExtras = {
                queryParams: { 'origin': this.router.url },
                fragment: 'anchor'
            };

            // Navigate to the login page with extras
            this.router.navigate(['/notauthorized'], navigationExtras);
        }

        return routeCanNavigate;
    }
}