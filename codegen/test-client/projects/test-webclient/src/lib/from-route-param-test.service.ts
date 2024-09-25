import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class FromRouteParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/from-routing-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("FromRouteParamTestService instance created");
	}
	async implicitRoute(name: string, id: number): Promise<any>  {
		const relativeUrl = `implicit-route/${name}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async explicitRoute(name: string, id: number): Promise<any>  {
		const relativeUrl = `explicit-route/${name}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async wildCardRouteDouble(name: string, id: number): Promise<any>  {
		const relativeUrl = `wild-card-route-double/${id}/${name}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async wildCardRouteSingle(name: string, id: number): Promise<any>  {
		const relativeUrl = `wild-card-route-single/${id}/${name}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async dateTimeRoute(date: string, id: number): Promise<any>  {
		const relativeUrl = `date-time-route/${date}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async dateTimeAsDateOnlyRoute(date: string, id: number): Promise<any>  {
		const relativeUrl = `date-time-as-date-only-route/${date}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async dateOnlyRoute(date: string, id: number): Promise<any>  {
		const relativeUrl = `date-only-route/${date}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async dateTimeOffsetRoute(date: DateTimeOffset, id: number): Promise<any>  {
		const relativeUrl = `datetimeoffset-route/${date}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async timeOnlyRoute(time: TimeOnly, id: number): Promise<any>  {
		const relativeUrl = `timeonly-route/${time}/${id}`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
}

