
import { ModuleWithProviders } from '@angular/core';
import { Routes }              from '@angular/router';
import { RouterModule }        from '@angular/router';

import { RfcListComponent }    from './rfc-list.component';
import { RfcAddComponent }     from './rfc-add.component';
import { RfcEditComponent }    from './rfc-edit.component';

const rfcRoutes: Routes = [
    { path: 'rfcs',     component: RfcListComponent }, 
    { path: 'add-rfc',  component: RfcAddComponent  }, 
    { path: 'edit-rfc', component: RfcEditComponent }
];

export const rfcRouting: ModuleWithProviders = RouterModule.forChild(rfcRoutes);

