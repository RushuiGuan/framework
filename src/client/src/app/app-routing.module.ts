import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { App1Component } from './app1/app1.component';
import { App2Component } from './app2/app2.component';
import { Page1Component } from './page1/page1.component';
import { Page2Component } from './page2/page2.component';
import { Page3Component } from './page3/page3.component';
import { Page4Component } from './page4/page4.component';

const routes: Routes = [
    {
        path: 'app1',
        component: App1Component,
        children: [
            {
                path: 'page1',
                component: Page1Component
            },
            {
                path: 'page2',
                component: Page2Component
            }
        ]
    },
    {
        path: 'app2',
        component: App2Component,
        children: [
            {
                path: 'page3',
                component: Page3Component
            },
            {
                path: 'page4',
                component: Page4Component
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
