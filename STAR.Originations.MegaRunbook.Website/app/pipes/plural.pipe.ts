
import { Pipe }          from '@angular/core';
import { PipeTransform } from '@angular/core';

/*
 * Takes a message argument and returns a 'plural' version of the message based on the number provided.
 *
 * Usage:
 *   value | plural:message
 * 
 * Example:
 *   {{ 0 |  plural:'banana' }} >>> formats to: '0 bananas'
 *   {{ 1 |  plural:'banana' }} >>> formats to: '1 banana'
 *   {{ 2 |  plural:'banana' }} >>> formats to: '2 bananas'
*/

@Pipe({ name: 'plural' })
export class PluralPipe implements PipeTransform {
    transform(value: number, message: string): string {
        var s = '';
        if (value === 1) {
            s = value + ' ' + message;
        } else {
            s = value + ' ' + message + 's';
        }
        return s;
    }
}
