
import { Component }  from '@angular/core';
import { OnInit }     from '@angular/core';
import { Router }     from '@angular/router';

import { Car }        from './car';
import { CarService } from './carservice';

@Component({
    templateUrl: 'app/prime/datatabledemo.html',
    providers: []
})

export class DataTableDemo implements OnInit {

    Title = "Prime NG - DataTable";

    cars: Car[];

    cols: any[];

    constructor(private carService: CarService) {
    }

    ngOnInit() {
        this.carService.getCarsSmall().then(cars => this.cars = cars);
        this.cols = [
            { field: 'vin',   header: 'Vin' },
            { field: 'year',  header: 'Year' },
            { field: 'brand', header: 'Brand' },
            { field: 'color', header: 'Color' }
        ];
    }

}