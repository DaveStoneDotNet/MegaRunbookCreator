
import { ApplicationLink } from './application-link.entity';

export class PagedApplicationLink {
    Items: ApplicationLink[];
    HasPrevious: boolean;
    HasNext: boolean;
    PageNumber: number;
    RecordsPerPage: number;
    TotalPageCount: number;
    TotalRecordCount: number;
}
