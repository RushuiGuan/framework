import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class ArrayParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/array-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("ArrayParamTestService instance created");
	}
	async arrayStringParam(array: string[]): Promise<string>  {
		const relativeUrl = `array-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { a});
		return result;
	}
	async arrayValueType(array: number[]): Promise<string>  {
		const relativeUrl = `array-value-type`;
		const result = await this.doGetStringAsync(relativeUrl, { a});
		return result;
	}
	async collectionStringParam(collection: string[]): Promise<string>  {
		const relativeUrl = `collection-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { c});
		return result;
	}
	async collectionValueType(collection: number[]): Promise<string>  {
		const relativeUrl = `collection-value-type`;
		const result = await this.doGetStringAsync(relativeUrl, { c});
		return result;
	}
}

