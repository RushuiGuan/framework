import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from '../signalr.service'

@Component({
    selector: 'app-app1',
    templateUrl: './app1.component.html',
    styleUrls: ['./app1.component.css'],
    providers: [SignalrService]
})
export class App1Component implements OnInit, OnDestroy {

    constructor(private svc: SignalrService) { }


    ngOnInit(): void {
        const conn = this.svc.start();
        conn.on("update", args => console.log("received notif", args));
    }
    ngOnDestroy(): void {
        this.svc.close();
    }
}
