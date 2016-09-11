
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
            this.service.getHero(id).then(hero => this.hero = hero);
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

}