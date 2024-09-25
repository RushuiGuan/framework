import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class RedirectTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/redirect-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("RedirectTestService instance created");
	}
	async get(): Promise<IActionResult>  {
		const relativeUrl = `test-0`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get1(): Promise<IActionResult>  {
		const relativeUrl = `test-1`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get2(): Promise<IActionResult>  {
		const relativeUrl = `test-2`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get3(): Promise<IActionResult>  {
		const relativeUrl = `test-3`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get4(): Promise<IActionResult>  {
		const relativeUrl = `test-4`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get5(): Promise<IActionResult>  {
		const relativeUrl = `test-5`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get6(): Promise<IActionResult>  {
		const relativeUrl = `test-6`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get7(): Promise<IActionResult>  {
		const relativeUrl = `test-7`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get8(): Promise<IActionResult>  {
		const relativeUrl = `test-8`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get9(): Promise<IActionResult>  {
		const relativeUrl = `test-9`;
		const result = await this.doGetAsync<IActionResult>(relativeUrl, null);
		return result;
	}
	async get10(): Promise<string>  {
		const relativeUrl = `test-10`;
		const result = await this.doGetStringAsync(relativeUrl, null);
		return result;
	}
}

