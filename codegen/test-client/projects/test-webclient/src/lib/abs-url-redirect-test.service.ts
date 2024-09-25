import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class AbsUrlRedirectTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/abs-url-redirect-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("AbsUrlRedirectTestService instance created");
	}
	async get(): Promise<any>  {
		const relativeUrl = `test-0`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get1(): Promise<any>  {
		const relativeUrl = `test-1`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get2(): Promise<any>  {
		const relativeUrl = `test-2`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get3(): Promise<any>  {
		const relativeUrl = `test-3`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get4(): Promise<any>  {
		const relativeUrl = `test-4`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get5(): Promise<any>  {
		const relativeUrl = `test-5`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get6(): Promise<any>  {
		const relativeUrl = `test-6`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get7(): Promise<any>  {
		const relativeUrl = `test-7`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get8(): Promise<any>  {
		const relativeUrl = `test-8`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get9(): Promise<any>  {
		const relativeUrl = `test-9`;
		const result = await this.doGetAsync<any>(relativeUrl, null);
		return result;
	}
	async get10(): Promise<string>  {
		const relativeUrl = `test-10`;
		const result = await this.doGetStringAsync(relativeUrl, null);
		return result;
	}
}

