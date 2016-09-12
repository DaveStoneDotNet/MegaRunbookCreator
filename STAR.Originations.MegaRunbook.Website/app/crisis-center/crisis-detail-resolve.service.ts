
import { Injectable }             from '@angular/core';
import { Router }                 from '@angular/router';
import { Resolve }                from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';

import { Crisis }                 from './crisis.service';
import { CrisisService }          from './crisis.service';

import { Observable }             from 'rxjs/Observable';

@Injectable()
export class CrisisDetailResolve implements Resolve<Crisis> {

    constructor(private cs: CrisisService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> | Promise<any> | any {

        let id = +route.params['id'];

        return this.cs.getCrisis(id).then(crisis => {
            if (crisis) {
                return crisis;
            } else {

                // id not found
                this.router.navigate(['/crisis-center']);
                return false;
            }
        });
    }
}