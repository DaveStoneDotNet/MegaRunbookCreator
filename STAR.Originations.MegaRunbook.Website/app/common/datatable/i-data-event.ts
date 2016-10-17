
import { SortEvent } from './i-sort-event';

export interface DataEvent {

    TotalRecordCount: number;

    PageNumber: number;
    RecordsPerPage: number;
    SortInfo: SortEvent;
}

