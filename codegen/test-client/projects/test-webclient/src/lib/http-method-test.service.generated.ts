import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class HttpMethodTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/http-method-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("HttpMethodTestService instance created");
	}
	delete(): Observable<object>  {
		const relativeUrl = ``;
		const result = this.doDeleteAsync(relativeUrl, {});
		return result;
	}
	post(): Observable<object>  {
		const relativeUrl = ``;
		const result = this.doPostAsync<object, string>(relativeUrl, "", {});
		return result;
	}
	patch(): Observable<object>  {
		const relativeUrl = ``;
		const result = this.doPatchAsync<object, string>(relativeUrl, "", {});
		return result;
	}
	get(): Observable<number>  {
		const relativeUrl = ``;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	put(): Observable<object>  {
		const relativeUrl = ``;
		const result = this.doPutAsync<object, string>(relativeUrl, "", {});
		return result;
	}
}
