import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { format }  from "date-fns";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class FromRouteParamTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/from-routing-param-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("FromRouteParamTestService instance created");
	}
	implicitRoute(name: string, id: number): Observable<object>  {
		const relativeUrl = `implicit-route/${name}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	explicitRoute(name: string, id: number): Observable<object>  {
		const relativeUrl = `explicit-route/${name}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	wildCardRouteDouble(name: string, id: number): Observable<object>  {
		const relativeUrl = `wild-card-route-double/${id}/${name}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	wildCardRouteSingle(name: string, id: number): Observable<object>  {
		const relativeUrl = `wild-card-route-single/${id}/${name}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	dateTimeRoute(date: Date, id: number): Observable<object>  {
		const relativeUrl = `date-time-route/${format(date, "yyyy-MM-ddTHH:mm:ssXXX")}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	dateTimeAsDateOnlyRoute(date: Date, id: number): Observable<object>  {
		const relativeUrl = `date-time-as-date-only-route/${format(date, "yyyy-MM-dd")}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	dateOnlyRoute(date: Date, id: number): Observable<object>  {
		const relativeUrl = `date-only-route/${format(date, "yyyy-MM-dd")}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	dateTimeOffsetRoute(date: Date, id: number): Observable<object>  {
		const relativeUrl = `datetimeoffset-route/${format(date, "yyyy-MM-ddTHH:mm:ssXXX")}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	timeOnlyRoute(time: Date, id: number): Observable<object>  {
		const relativeUrl = `timeonly-route/${format(time, "HH:mm:ss.SSS")}/${id}`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
}
