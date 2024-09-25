import { MyDto }  from './dto';
import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class RequiredParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/required-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("RequiredParamTestService instance created");
	}
	async explicitStringParam(text: string): Promise<string>  {
		const relativeUrl = `explicit-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { text});
		return result;
	}
	async implicitStringParam(text: string): Promise<string>  {
		const relativeUrl = `implicit-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { text});
		return result;
	}
	async requiredStringParam(text: string): Promise<string>  {
		const relativeUrl = `required-string-param`;
		const result = await this.doGetStringAsync(relativeUrl, { text});
		return result;
	}
	async requiredValueType(id: number): Promise<string>  {
		const relativeUrl = `required-value-type`;
		const result = await this.doGetStringAsync(relativeUrl, { id});
		return result;
	}
	async requiredDateOnly(date: string): Promise<string>  {
		const relativeUrl = `required-date-only`;
		const result = await this.doGetStringAsync(relativeUrl, { date});
		return result;
	}
	async requiredDateTime(date: string): Promise<string>  {
		const relativeUrl = `required-datetime`;
		const result = await this.doGetStringAsync(relativeUrl, { date});
		return result;
	}
	async requiredDateTimeAsDateOnly(date: string): Promise<string>  {
		const relativeUrl = `requried-datetime-as-dateonly`;
		await this.doDeleteAsync(relativeUrl, { date})
	}
	async requiredPostParam(dto: MyDto): Promise<any>  {
		const relativeUrl = `required-post-param`;
		const result = await this.doPostAsync<any,MyDto>(relativeUrl, dto, null);
		return result;
	}
	async requiredStringArray(values: string[]): Promise<string>  {
		const relativeUrl = `required-string-array`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async requiredStringCollection(values: string[]): Promise<string>  {
		const relativeUrl = `required-string-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async requiredValueTypeArray(values: number[]): Promise<string>  {
		const relativeUrl = `required-value-type-array`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async requiredValueTypeCollection(values: number[]): Promise<string>  {
		const relativeUrl = `required-value-type-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { values});
		return result;
	}
	async requiredDateOnlyCollection(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-date-only-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async requiredDateOnlyArray(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-date-only-array`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async requiredDateTimeCollection(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-datetime-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async requiredDateTimeArray(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-datetime-array`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async requiredDateTimeAsDateOnlyCollection(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-datetime-as-dateonly-collection`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
	async requiredDateTimeAsDateOnlyArray(dates: Date[]): Promise<string>  {
		const relativeUrl = `required-datetime-as-dateonly-array`;
		const result = await this.doGetStringAsync(relativeUrl, { dates});
		return result;
	}
}

