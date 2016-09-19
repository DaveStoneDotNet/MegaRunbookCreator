
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

    powers = ['Smart', 'Creative', 'Inspirational', 'Talented'];

    hero: Hero = new Hero(0, '', '', '');

    active = true;
    submitted = false;

    onSubmit(form: any) {
        this.submitted = true;
        console.log(form.name);
    }

    get diagnostic() { return JSON.stringify(this.hero); }

    newHero() {

        this.hero = new Hero(0, '', '');

        // Reset the form with a new hero and restore the 'pristine' state by toggling the 'active' flag.
        // The NgIf reference in the HTML causes the form to be removed from the DOM and then immediately re-added 
        // which has the effect of changing the form state back to 'pristine'.

        // This a temporary hack until a proper form reset feature is added to Angular.

        this.active = false;
        setTimeout(() => this.active = true, 0);

    }

    showFormControls(form: any) {

        return form && form.controls['name'] && form.controls['name'].value;
    }

    // ----------------------------------------------------------------------------------------------


    Title = "Hero Detail";

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

    formIsValid() {
        return true;
    }

    private onHeroSuccessful(hero: Hero) {
        this.hero = hero;
    }
}