
import { RFC } from './rfc.entity';

export class PagedRfc {
    Items: RFC[];
    HasPrevious: boolean;
    HasNext: boolean;
    PageNumber: number;
    RecordsPerPage: number;
    TotalPageCount: number;
    TotalRecordCount: number;
}
