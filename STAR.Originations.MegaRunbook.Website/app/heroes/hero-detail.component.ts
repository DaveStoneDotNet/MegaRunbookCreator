
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

    powers = ['Really Smart', 'Super Flexible', 'Super Hot', 'Weather Changer'];

    model = new Hero(18, 'Dr IQ', this.powers[0], 'Chuck Overstreet');

    submitted = false;

    onSubmit() { this.submitted = true; }

    // TODO: Remove this when we're done
    get diagnostic() { return JSON.stringify(this.model); }

    // Reset the form with a new hero AND restore 'pristine' class state
    // by toggling 'active' flag which causes the form
    // to be removed/re-added in a tick via NgIf
    // TODO: Workaround until NgForm has a reset method (#6822)
    active = true;

    newHero() {
        this.model = new Hero(42, '', '');
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }
    //////// NOT SHOWN IN DOCS ////////

    // Reveal in html:
    //   Name via form.controls = {{showFormControls(heroForm)}}
    showFormControls(form: any) {

        return form && form.controls['name'] && form.controls['name'].value;
    }


    // ----------------------------------------------------------------------------------------------


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