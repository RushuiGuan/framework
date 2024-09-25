import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class HttpMethodTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/http-method-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("HttpMethodTestService instance created");
	}
	async delete(): Promise<any>  {
		const relativeUrl = ``;
		await this.doDeleteAsync(relativeUrl, null)
	}
	async post(): Promise<any>  {
		const relativeUrl = ``;
		const result = await this.doPostAsync<any,any>(relativeUrl, null, null);
		return result;
	}
	async patch(): Promise<any>  {
		const relativeUrl = ``;
	}
	async get(): Promise<number>  {
		const relativeUrl = ``;
		const result = await this.doGetAsync<number>(relativeUrl, null);
		return result;
	}
	async put(): Promise<any>  {
		const relativeUrl = ``;
	}
}

