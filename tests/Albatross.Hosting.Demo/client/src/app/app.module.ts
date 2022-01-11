import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APP_BASE_HREF, CommonModule, PlatformLocation } from '@angular/common';
import { ConfigService } from './config.service';
import { App1Component } from './app1/app1.component';
import { App2Component } from './app2/app2.component';
import { Page1Component } from './page1/page1.component';
import { Page2Component } from './page2/page2.component';
import { Page3Component } from './page3/page3.component';
import { Page4Component } from './page4/page4.component';

export function initApp(cfgSvc: ConfigService): any {
	console.log("app initialization");
	const promise = cfgSvc.init();
	return () => promise;
}
export function getBaseHref(platformLocation: PlatformLocation): string {
	return platformLocation.getBaseHrefFromDOM();
}

@NgModule({
	declarations: [
		AppComponent,
		App1Component,
		App2Component,
		Page1Component,
		Page2Component,
		Page3Component,
		Page4Component
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		CommonModule,
		HttpClientModule
	],
	providers: [
		{
			provide: APP_BASE_HREF,
			useFactory: getBaseHref,
			deps: [PlatformLocation]
		},
		{
			provide: APP_INITIALIZER,
			useFactory: initApp,
			deps: [ConfigService], multi: true
		}
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
