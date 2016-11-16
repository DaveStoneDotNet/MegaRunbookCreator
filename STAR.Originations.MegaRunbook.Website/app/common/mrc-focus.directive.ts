
import { Directive }    from '@angular/core';
import { Input }        from '@angular/core';
import { EventEmitter } from '@angular/core';
import { ElementRef }   from '@angular/core';
import { Renderer }     from '@angular/core';
import { Inject }       from '@angular/core';


@Directive({
    selector: '[focus]'
})
export class MrcFocusDirective {
    @Input('focus') focusEvent: EventEmitter<boolean>;

    constructor( @Inject(ElementRef) private element: ElementRef, private renderer: Renderer) {
    }

    ngOnInit() {
        this.focusEvent.subscribe(event => {
            this.renderer.invokeElementMethod(this.element.nativeElement, 'focus', []);
        });
    }
}