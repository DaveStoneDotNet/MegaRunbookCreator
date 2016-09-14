
import { Component }      from '@angular/core';
import { OnInit }         from '@angular/core';
import { OnDestroy }      from '@angular/core';

import { Router }         from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { Hero }           from './hero';
import { HeroService }    from './hero.service';

import { Subscription }   from 'rxjs/Subscription';

@Component({
    templateUrl: 'app/heroes/hero-list.component.html',
    providers:   [HeroService]
})

export class HeroListComponent implements OnInit, OnDestroy {

    Title = "Heroes";
    heroes: Hero[];

    private selectedId: number;
    private subscription: Subscription;

    constructor(private router: Router, private route: ActivatedRoute, private service: HeroService) { }

    ngOnInit() {
        this.subscription = this.route
            .params
            .subscribe(params => {
                this.selectedId = +params['id'];
                this.service.getHeroes()
                    .then(heroes => this.onHeroesSuccessful(heroes));
            });
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    onSelect(hero: Hero) {
        this.router.navigate(['/hero', hero.Id]);
    }

    isSelected(hero: Hero) {
        return hero.Id === this.selectedId;
    }

    private onHeroesSuccessful(response: Hero[]) {
        this.heroes = response;
    }

}