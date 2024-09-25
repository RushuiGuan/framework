import { MyDto }  from './dto';
import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class NullableParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/nullable-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("NullableParamTestService instance created");
	}
	async nullableStringParam(text: string): Promise<string>  {
		const relativeUrl = `nullable-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { text});
		return result;
	}
	async nullableValueType(id: number): Promise<string>  {
		const relativeUrl = `nullable-value-type`;
		const result = await this.doGetStringAsync(relativeUrl, { id});
		return result;
	}
	async nullableDateOnly(date: string): Promise<string>  {
		const relativeUrl = `nullable-date-only`;
		const result = await this.doGetStringAsync(relativeUrl, { date});
		return result;
	}
	async nullablePostParam(dto: MyDto): Promise<any>  {
		const relativeUrl = `nullable-post-param`;
		const result = await this.doPostAsync<any,MyDto>(relativeUrl, dto, null);
		return result;
	}
	async nullableStringArray(values: string[]): Promise<string>  {
		const relativeUrl = `nullable-string-array`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async nullableStringCollection(values: string[]): Promise<string>  {
		const relativeUrl = `nullable-string-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async nullableValueTypeArray(values: number[]): Promise<string>  {
		const relativeUrl = `nullable-value-type-array`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async nullableValueTypeCollection(values: number[]): Promise<string>  {
		const relativeUrl = `nullable-value-type-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async nullableDateOnlyCollection(dates: Date[]): Promise<string>  {
		const relativeUrl = `nullable-date-only-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async nullableDateOnlyArray(dates: Date[]): Promise<string>  {
		const relativeUrl = `nullable-date-only-array`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
}

