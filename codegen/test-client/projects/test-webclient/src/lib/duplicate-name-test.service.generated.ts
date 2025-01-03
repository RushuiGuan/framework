import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class DuplicateNameTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/duplicate-name-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("DuplicateNameTestService instance created");
	}
	submit(id: number): Observable<object>  {
		const relativeUrl = `by-id`;
		const result = this.doPostAsync<object, string>(relativeUrl, "", { id });
		return result;
	}
	submit1(name: string): Observable<object>  {
		const relativeUrl = `by-name`;
		const result = this.doPostAsync<object, string>(relativeUrl, "", { name });
		return result;
	}
}
