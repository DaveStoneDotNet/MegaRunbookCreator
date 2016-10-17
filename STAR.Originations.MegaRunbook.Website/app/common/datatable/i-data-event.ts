
import { SortEvent } from './i-sort-event';

export interface DataEvent {
    length: number;

    PageNumber: number;
    PageSize: number;
    SortInfo: SortEvent;
}

