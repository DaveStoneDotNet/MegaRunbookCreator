
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

    Title = "Hero List";

    heroes: Hero[];

    private selectedId: number;
    private sub: Subscription;

    constructor(private router: Router, private route: ActivatedRoute, private service: HeroService) { }

    ngOnInit() {
        //this.sub = this.route
        //    .params
        //    .subscribe(params => {
        //        this.selectedId = +params['id'];
        //        this.service.getHeroes()
        //            .then(heroes => this.heroes = heroes);
        //    });
    }

    ngOnDestroy() {
        //this.sub.unsubscribe();
    }

    onSelect(hero: Hero) {
        this.router.navigate(['/hero', hero.id]);
    }

    isSelected(hero: Hero) {
        return hero.id === this.selectedId;
    }
}