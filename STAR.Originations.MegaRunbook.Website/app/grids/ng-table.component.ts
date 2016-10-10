
import { Component }           from '@angular/core';
import { OnInit }              from '@angular/core';
import { Router }              from '@angular/router';

import { NG_TABLE_DIRECTIVES } from 'ng2-table';
import { PaginationModule }    from 'ng2-bootstrap/components/pagination';

import { TableData }           from './table-data';

@Component({
    templateUrl: 'app/grids/ng-table.component.html',
    providers:    []
})

export class NgTableComponent implements OnInit {

    Title = "NG-Table";

    // --------------------------------------------------------------------------

    private data: Array<any> = TableData;

    rows: Array<any> = [];

    columns: Array<any> = [
        { title: 'Name',       name: 'name',     sort: 'asc' },
        { title: 'Position',   name: 'position', sort: false },
        { title: 'Office',     name: 'office',               },
        { title: 'Extn.',      name: 'ext',                  },
        { title: 'Start date', name: 'startDate'             },
        { title: 'Salary ($)', name: 'salary'                }
    ];

    config: any = {
        paging:    true,
        sorting:   { columns: this.columns },
        filtering: { filterString: '', columnName: 'position' }
    };

    page:         number = 1;
    itemsPerPage: number = 10;
    maxSize:      number = 5;
    numPages:     number = 1;
    length:       number = 0;

    changeFilter(data: any, config: any): any {
        if (!config.filtering) {
            return data;
        }

        let filteredData: Array<any> = data.filter((item: any) =>
            item[config.filtering.columnName].match(this.config.filtering.filterString));

        return filteredData;
    }

    changeSort(data: any, config: any): any {
        if (!config.sorting) {
            return data;
        }

        let columns = this.config.sorting.columns || [];
        let columnName: string = void 0;
        let sort: string = void 0;

        for (let i = 0; i < columns.length; i++) {
            if (columns[i].sort !== '') {
                columnName = columns[i].name;
                sort = columns[i].sort;
            }
        }

        if (!columnName) {
            return data;
        }

        // simple sorting
        return data.sort((previous: any, current: any) => {
            if (previous[columnName] > current[columnName]) {
                return sort === 'desc' ? -1 : 1;
            } else if (previous[columnName] < current[columnName]) {
                return sort === 'asc' ? -1 : 1;
            }
            return 0;
        });
    }

    changePage(page: any, data: Array<any> = this.data): Array<any> {
        console.log(page);
        let start = (page.page - 1) * page.itemsPerPage;
        let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
        return data.slice(start, end);
    }

    onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
        if (config.filtering) {
            Object.assign(this.config.filtering, config.filtering);
        }
        if (config.sorting) {
            Object.assign(this.config.sorting, config.sorting);
        }

        let filteredData = this.changeFilter(this.data, this.config);
        let sortedData = this.changeSort(filteredData, this.config);
        this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
        this.length = sortedData.length;
    }

    // --------------------------------------------------------------------------

    constructor() {
        this.length = this.data.length;
    }

    ngOnInit() {
        this.onChangeTable(this.config);
    }

}