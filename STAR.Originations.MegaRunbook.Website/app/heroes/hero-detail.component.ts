
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { OnDestroy }      from '@angular/core';

import { Router }         from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { Hero }           from './hero';
import { HeroService }    from './hero.service';

import { Subscription }   from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/heroes/hero-detail.component.html',
    providers: []
})

export class HeroDetailComponent implements OnInit, OnDestroy {

    Title = "Hero Detail";
    hero: Hero;

    private sub: Subscription;

    constructor(private router: Router, private route: ActivatedRoute, private service: HeroService) { }

    ngOnInit() {

        // (+) converts string 'id' to a number
        this.sub = this.route.params.subscribe(params => {
            let id = +params['id'];
            this.service.getHero({ id: id }).then(hero => this.onHeroSuccessful(hero));
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    gotoHeroes() {

        let heroId = this.hero ? this.hero.Id : null;

        // Pass along the hero id if available so that the HeroList component can select that hero.
        this.router.navigate(['/heroes', { id: heroId }]);
    }

    private onHeroSuccessful(hero: Hero) {
        this.hero = hero;
    }

}