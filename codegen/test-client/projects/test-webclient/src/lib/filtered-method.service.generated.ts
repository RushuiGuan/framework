import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class FilteredMethodService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/filtered-method";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("FilteredMethodService instance created");
	}
	filteredByNone(): Observable<object>  {
		const relativeUrl = `none`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	filteredByCSharp(): Observable<object>  {
		const relativeUrl = `csharp`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	filteredByTypeScript(): Observable<object>  {
		const relativeUrl = `typescript`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
}
