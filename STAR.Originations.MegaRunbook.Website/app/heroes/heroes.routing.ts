
import { ModuleWithProviders }      from '@angular/core';
import { Routes }                   from '@angular/router';
import { RouterModule }             from '@angular/router';

import { HeroListComponent }        from './hero-list.component'
import { HeroDetailComponent }      from './hero-detail.component'

const heroesRoutes: Routes = [
    { path: 'heroes', component: HeroListComponent, data: { Title: 'Hero List' } },
    { path: 'hero/:id', component: HeroDetailComponent }
];

export const heroesRouting: ModuleWithProviders = RouterModule.forRoot(heroesRoutes);

