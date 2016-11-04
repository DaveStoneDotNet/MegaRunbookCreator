
(function (global) {
    System.config({

        paths: {
            // paths serve as alias
            'npm:': 'node_modules/'
        },

        // 'map' tells the System loader where to look for things
        map: {

            // our app is within the app folder
            app: 'app',

            // angular bundles
            '@angular/core':                     'npm:@angular/core/bundles/core.umd.js',
            '@angular/common':                   'npm:@angular/common/bundles/common.umd.js',
            '@angular/compiler':                 'npm:@angular/compiler/bundles/compiler.umd.js',
            '@angular/platform-browser':         'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
            '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
            '@angular/http':                     'npm:@angular/http/bundles/http.umd.js',
            '@angular/router':                   'npm:@angular/router/bundles/router.umd.js',
            '@angular/forms':                    'npm:@angular/forms/bundles/forms.umd.js',

            // other libraries
            'rxjs':                              'npm:rxjs',
            'primeng':                           'npm:primeng',
            'moment':                            'npm:moment',
            'ng2-table':                         'npm:ng2-table',
            'ng2-bootstrap':                     'npm:ng2-bootstrap',
            'angular2-in-memory-web-api':        'npm:angular2-in-memory-web-api',
            'clipboard':                         'npm:clipboard/dist/clipboard.js',
            'angular2-clipboard':                'npm:angular2-clipboard',
            'toastr-ng2':                        'npm:toastr-ng2'
        },

        // 'packages' tells the System loader how to load when no filename and/or no extension
        packages: {
            app: {
                main: './main.js',
                defaultExtension: 'js'
            },
            rxjs: {
                defaultExtension: 'js'
            },
            'primeng': {
                main: 'primeng.js',
                defaultExtension: 'js'
            },
            moment: {
                main: 'moment.js',
                defaultExtension: 'js'
            },
            'ng2-table': {
                main: 'ng2-table.js',
                defaultExtension: 'js'
            },
            'ng2-bootstrap': {
                main: 'ng2-bootstrap.js',
                defaultExtension: 'js'
            },
            'angular2-in-memory-web-api': {
                main: './index.js',
                defaultExtension: 'js'
            }, 
            'angular2-clipboard': {
                main: 'index.js',
                defaultExtension: 'js'
            },
            'clipboard': {
                defaultExtension: 'js'
            }, 
            'toastr-ng2': {
                main: 'toastr.js',
                defaultExtension: 'js'
            }
        }
    });
})(this);
