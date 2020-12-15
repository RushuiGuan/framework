import { Injectable } from '@angular/core';
import { IHttpConnectionOptions, HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

@Injectable()
export class SignalrService {
    private hubConnection: HubConnection = null;

    constructor() {
        console.log("SignalrService instance created");
    }

    start(): HubConnection {
        if (!this.hubConnection) {
            console.log("SignalrService instance started", name);
            let signalrOption: IHttpConnectionOptions = {
                withCredentials: true
            };
            this.hubConnection = new HubConnectionBuilder()
                .withUrl("http://localhost/demo/notif", { withCredentials: true })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();
            this.hubConnection.start();
        }
        return this.hubConnection;
    }

    close() {
        this.hubConnection.stop();
        console.log("SignalrService instance closed");
    }
}
