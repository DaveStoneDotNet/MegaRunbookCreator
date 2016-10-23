
import { NgModule }    from '@angular/core';
import { PluralPipe }  from '../pipes/plural.pipe';

@NgModule({
    imports:      [],
    declarations: [PluralPipe],
    exports:      [PluralPipe]
})
export class PipeModule { }
