
import { Injectable }             from '@angular/core';
import { CanActivate }            from '@angular/router';
import { Router }                 from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { RouterStateSnapshot }    from '@angular/router';
import { NavigationExtras }       from '@angular/router';

import { AuthService }            from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        if (this.authService.isLoggedIn) {
             return true;
        }

        // Store the attempted URL for redirecting
        this.authService.redirectUrl = state.url;

        // Create a dummy session id
        let sessionId = 123456789;

        // Set our navigation extras object that contains our global query params and fragment
        let navigationExtras: NavigationExtras = {
            queryParams: { 'session_id': sessionId },
            fragment: 'anchor'
        };

        // Navigate to the login page with extras
        this.router.navigate(['/login'], navigationExtras);
        return false;
    }
}