import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class CancellationTokenTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/cancellationtokentest";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("CancellationTokenTestService instance created");
	}
	get(cancellationToken: CancellationToken): Observable<string>  {
		const relativeUrl = ``;
		const result = this.doGetStringAsync(relativeUrl, { cancellationToken });
		return result;
	}
}
