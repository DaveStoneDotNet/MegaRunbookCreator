
import { SortInfo } from './i-sort-info';

export interface DataEvent {

    PageNumber:       number;
    RecordsPerPage:   number;
    TotalPageCount:   number;
    TotalRecordCount: number;

    SortInfo:         SortInfo;

    Items:            any[];
}

