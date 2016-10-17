export interface PagedData {

    PageNumber:       number;
    RecordsPerPage:   number;
    TotalPageCount:   number;
    TotalRecordCount: number;

    Items:            any[];
}

