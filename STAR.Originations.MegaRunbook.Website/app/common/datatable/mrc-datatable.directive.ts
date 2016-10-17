
import { Directive }       from '@angular/core';
import { OnInit }          from '@angular/core';
import { Input }           from '@angular/core';
import { Output }          from '@angular/core';
import { EventEmitter }    from '@angular/core';
import { SimpleChange }    from '@angular/core';
import { OnChanges }       from '@angular/core';
import { DoCheck }         from '@angular/core';

import { DataEvent }       from './i-data-event';
import { PageEvent }       from './i-page-event';
import { SortInfo }        from './i-sort-info';

import { MrcSortSettings } from './mrc-sort-settings';

@Directive({
    selector: 'table[mrc-data-input]',
    exportAs: 'mrcDataTable'                    // The name under which the component instance is exported in a template
})
export class DataTable implements OnInit, OnChanges, DoCheck {

    @Input('mrc-data-input')       public mrcDataInput: DataEvent;
    @Input('mrc-records-per-page') public recordsPerPage: number;
    @Input('mrc-current-page')     public currentPage = 1;

    @Output() onDataRequested = new EventEmitter<DataEvent>();
    @Output() onDataReceived  = new EventEmitter<DataEvent>();

    onDataChange = new EventEmitter<DataEvent>();
    onSortChange = new EventEmitter<SortInfo>();
    onPageChange = new EventEmitter<PageEvent>(); 

    // ---------------------------------------------------------------------------------

    //public PropertyName: string;
    //public SortOrder = MrcSortSettings.ASCENDING;

    // ---------------------------------------------------------------------------------

    public dataEvent: DataEvent;
    public pageEvent: PageEvent;
    public sortInfo:  SortInfo;

    // ---------------------------------------------------------------------------------

    ngOnInit() {
        this.dataEvent = {
            PageNumber:       this.currentPage,
            RecordsPerPage:   this.recordsPerPage,
            TotalPageCount:   0,
            TotalRecordCount: this.mrcDataInput.TotalRecordCount,
            SortInfo:         this.sortInfo,
            Items:            null
        };
    }

    //getDataEvent(): DataEvent {
    //    this.dataEvent = {
    //                PageNumber:       this.currentPage,
    //                RecordsPerPage:   this.recordsPerPage, 
    //                TotalPageCount:   0, 
    //                TotalRecordCount: this.mrcDataInput.TotalRecordCount,
    //                SortInfo:         this.sortInfo, 
    //                Items:            null
    //    };
    //    return this.dataEvent;
    //}

    //getSortInfo(): SortInfo {
    //    return this.sortInfo;
    //}

    setSortInfo(sortOrder: string, propertyName: string): void {

        if (propertyName && propertyName.length != 0) {
            this.sortInfo = { SortOrder: sortOrder, PropertyName: propertyName };
            if (this.dataEvent) {
                this.dataEvent.SortInfo = { SortOrder: sortOrder, PropertyName: propertyName };
            }
        } else {
            this.sortInfo = null;
            if (this.dataEvent) {
                this.dataEvent.SortInfo = null;
            }
        }
        this.onSortChange.emit(this.sortInfo);

        //this.onDataRequested.emit(this.getDataEvent());
        this.onDataRequested.emit(this.dataEvent);
   }

    //getPage(): PageEvent {
    //    return {
    //        currentPage:      this.currentPage,
    //        recordsPerPage:   this.recordsPerPage,
    //        totalRecordCount: this.mrcDataInput.TotalRecordCount
    //    };
    //}

    getPage(): PageEvent {
        return {
            currentPage:      this.dataEvent.PageNumber,
            recordsPerPage:   this.dataEvent.RecordsPerPage,
            totalRecordCount: this.dataEvent.TotalRecordCount
        };
    }

    setPage(currentPage: number, recordsPerPage: number): void {

        if (this.recordsPerPage !== recordsPerPage || this.currentPage !== currentPage) {

            // --------------------------------------------------------------------------

            //this.currentPage = this.currentPage = currentPage;

            this.currentPage    = currentPage;
            this.recordsPerPage = recordsPerPage;

            if (this.dataEvent) {
                this.dataEvent.PageNumber     = currentPage;
                this.dataEvent.RecordsPerPage = recordsPerPage;
            }

            // --------------------------------------------------------------------------

            //this.onPageChange.emit({
            //    currentPage:      this.currentPage,
            //    recordsPerPage:   this.recordsPerPage,
            //    totalRecordCount: this.mrcDataInput.TotalRecordCount
            //});

            this.onPageChange.emit({
                currentPage:      this.dataEvent.PageNumber,
                recordsPerPage:   this.dataEvent.RecordsPerPage,
                totalRecordCount: this.mrcDataInput.TotalRecordCount
            });

            //this.onDataRequested.emit(this.getDataEvent());
            this.onDataRequested.emit(this.dataEvent);
        }
    }

    ngOnChanges(changes: { [key: string]: SimpleChange }): any {

        if (changes['mrcDataInput']) {

            this.mrcDataInput = changes['mrcDataInput'].currentValue || { Items: [] };

            // --------------------------------------------------------------------------
            if (this.dataEvent) {
                this.dataEvent.PageNumber = this.mrcDataInput.PageNumber;
                this.dataEvent.RecordsPerPage = this.mrcDataInput.RecordsPerPage;
                this.dataEvent.TotalRecordCount = this.mrcDataInput.TotalRecordCount;
            }

            // --------------------------------------------------------------------------

            this.onPageChange.emit({
                currentPage:      this.mrcDataInput.PageNumber,
                recordsPerPage:   this.mrcDataInput.RecordsPerPage,
                totalRecordCount: this.mrcDataInput.TotalRecordCount
            });
        }
    }

    ngDoCheck(): any {

    }
}