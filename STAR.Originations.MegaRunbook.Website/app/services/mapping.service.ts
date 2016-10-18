
import { Injectable } from '@angular/core';

import { DataEvent }  from '../common/datatable/i-data-event';
import { Paging }     from '../entities/paging.entity';

@Injectable()
export class MappingService {

    constructor() {
    }

    dataEventToPaging(dataEvent: DataEvent, recordsPerPage: number): Paging {

        let paging = new Paging();
        if (dataEvent != null) {
            paging.PageNumber = dataEvent.PageNumber;
            paging.RecordsPerPage = dataEvent.RecordsPerPage;

            if (dataEvent.SortInfo) {
                if (dataEvent.SortInfo.PropertyName && dataEvent.SortInfo.PropertyName.length > 0) {
                    paging.SortInfo = [];
                    paging.SortInfo.push(dataEvent.SortInfo);
                }
            }
        } else {
            paging.PageNumber = 1;
            paging.RecordsPerPage = recordsPerPage;
        }

        return paging;
    }

}