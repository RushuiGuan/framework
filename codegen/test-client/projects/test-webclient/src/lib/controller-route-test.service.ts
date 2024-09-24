import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";

@Injectable({ providedIn: "root" })
export class ControllerRouteTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/controllerroutetest";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("ControllerRouteTestService instance created");
	}
}
