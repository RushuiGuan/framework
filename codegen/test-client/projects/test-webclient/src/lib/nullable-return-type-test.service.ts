import { MyDto }  from './dto';
import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class NullableReturnTypeTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/nullable-return-type/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("NullableReturnTypeTestService instance created");
	}
	async getString(): Promise<string>  {
		const relativeUrl = `string`;
		const result = await this.doGetStringAsync(relativeUrl, null);
		return result;
	}
	async getAsyncString(): Promise<string>  {
		const relativeUrl = `async-string`;
		const result = await this.doGetStringAsync(relativeUrl, null);
		return result;
	}
	async getActionResultString(): Promise<ActionResult_<string>>  {
		const relativeUrl = `action-result-string`;
		const result = await this.doGetAsync<ActionResult_<string>>(relativeUrl, null);
		return result;
	}
	async getAsyncActionResultString(): Promise<ActionResult_<string>>  {
		const relativeUrl = `async-action-result-string`;
		const result = await this.doGetAsync<ActionResult_<string>>(relativeUrl, null);
		return result;
	}
	async getInt(): Promise<number>  {
		const relativeUrl = `int`;
		const result = await this.doGetAsync<number>(relativeUrl, null);
		return result;
	}
	async getAsyncInt(): Promise<number>  {
		const relativeUrl = `async-int`;
		const result = await this.doGetAsync<number>(relativeUrl, null);
		return result;
	}
	async getActionResultInt(): Promise<ActionResult_<number>>  {
		const relativeUrl = `action-result-int`;
		const result = await this.doGetAsync<ActionResult_<number>>(relativeUrl, null);
		return result;
	}
	async getAsyncActionResultInt(): Promise<ActionResult_<number>>  {
		const relativeUrl = `async-action-result-int`;
		const result = await this.doGetAsync<ActionResult_<number>>(relativeUrl, null);
		return result;
	}
	async getDateTime(): Promise<Date>  {
		const relativeUrl = `datetime`;
		const result = await this.doGetAsync<Date>(relativeUrl, null);
		return result;
	}
	async getAsyncDateTime(): Promise<Date>  {
		const relativeUrl = `async-datetime`;
		const result = await this.doGetAsync<Date>(relativeUrl, null);
		return result;
	}
	async getActionResultDateTime(): Promise<ActionResult_<Date>>  {
		const relativeUrl = `action-result-datetime`;
		const result = await this.doGetAsync<ActionResult_<Date>>(relativeUrl, null);
		return result;
	}
	async getAsyncActionResultDateTime(): Promise<ActionResult_<Date>>  {
		const relativeUrl = `async-action-result-datetime`;
		const result = await this.doGetAsync<ActionResult_<Date>>(relativeUrl, null);
		return result;
	}
	async getMyDto(): Promise<MyDto>  {
		const relativeUrl = `object`;
		const result = await this.doGetAsync<MyDto>(relativeUrl, null);
		return result;
	}
	async getAsyncMyDto(): Promise<MyDto>  {
		const relativeUrl = `async-object`;
		const result = await this.doGetAsync<MyDto>(relativeUrl, null);
		return result;
	}
	async actionResultObject(): Promise<ActionResult_<MyDto>>  {
		const relativeUrl = `action-result-object`;
		const result = await this.doGetAsync<ActionResult_<MyDto>>(relativeUrl, null);
		return result;
	}
	async asyncActionResultObject(): Promise<ActionResult_<MyDto>>  {
		const relativeUrl = `async-action-result-object`;
		const result = await this.doGetAsync<ActionResult_<MyDto>>(relativeUrl, null);
		return result;
	}
	async getMyDtoNullableArray(): Promise<MyDto[]>  {
		const relativeUrl = `nullable-array-return-type`;
		const result = await this.doGetAsync<MyDto[]>(relativeUrl, null);
		return result;
	}
	async getMyDtoCollection(): Promise<MyDto[]>  {
		const relativeUrl = `nullable-collection-return-type`;
		const result = await this.doGetAsync<MyDto[]>(relativeUrl, null);
		return result;
	}
}

