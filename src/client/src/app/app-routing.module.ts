import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DemoComponent } from './demo/demo.component';
import { TestComponent } from './test/test.component';

const routes: Routes = [
    {
        path: 'test',
        component: TestComponent,
    },
    {
        path: 'demo',
        component: DemoComponent,
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
