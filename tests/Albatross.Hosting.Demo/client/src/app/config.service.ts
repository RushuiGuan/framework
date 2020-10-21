import { Inject, Injectable } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';

export class Config {
    environment: string;
    constructor(src: Config) {
        if (src) {
            this.environment = src.environment;
        }
    }
}

@Injectable({ providedIn: 'root' })
export class ConfigService {
    private config: Config;

    constructor(@Inject(APP_BASE_HREF) public baseHref: string) {
        console.log("ConfigService instance created with baseHref of", this.baseHref);
    }

    async init(): Promise<Config> {
        const request = new Request(`${this.baseHref}assets/config.json`);
        const myHeaders = new Headers();
        myHeaders.append('pragma', 'no-cache');
        myHeaders.append('cache-control', 'no-cache');
        const myInit = {
            method: 'GET',
            headers: myHeaders,
        };

        const src = await fetch(request, myInit)
            .then<Config>(response => response.json());

        this.config = new Config(src);
        return this.config;
    }

    get(): Config { return this.config; }
}
