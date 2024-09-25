import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class FromQueryParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/from-query-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("FromQueryParamTestService instance created");
	}
	async requiredString(name: string): Promise<any>  {
		const relativeUrl = `required-string`;
		const result = await this.doGetAsync<any>(relativeUrl, { name});
		return result;
	}
	async requiredStringImplied(name: string): Promise<any>  {
		const relativeUrl = `required-string-implied`;
		const result = await this.doGetAsync<any>(relativeUrl, { name});
		return result;
	}
	async requiredStringDiffName(name: string): Promise<any>  {
		const relativeUrl = `required-string-diff-name`;
		const result = await this.doGetAsync<any>(relativeUrl, { n});
		return result;
	}
	async requiredDateTime(datetime: string): Promise<any>  {
		const relativeUrl = `required-datetime`;
		const result = await this.doGetAsync<any>(relativeUrl, { datetime});
		return result;
	}
	async requiredDateTimeDiffName(datetime: string): Promise<any>  {
		const relativeUrl = `required-datetime_diff-name`;
		const result = await this.doGetAsync<any>(relativeUrl, { d});
		return result;
	}
	async requiredDateOnly(dateonly: string): Promise<any>  {
		const relativeUrl = `required-dateonly`;
		const result = await this.doGetAsync<any>(relativeUrl, { dateonly});
		return result;
	}
	async requiredDateOnlyDiffName(dateonly: string): Promise<any>  {
		const relativeUrl = `required-dateonly_diff-name`;
		const result = await this.doGetAsync<any>(relativeUrl, { d});
		return result;
	}
	async requiredDateTimeOffset(dateTimeOffset: DateTimeOffset): Promise<any>  {
		const relativeUrl = `required-datetimeoffset`;
		const result = await this.doGetAsync<any>(relativeUrl, { dateTimeOffset});
		return result;
	}
	async requiredDateTimeOffsetDiffName(dateTimeOffset: DateTimeOffset): Promise<any>  {
		const relativeUrl = `required-datetimeoffset_diff-name`;
		const result = await this.doGetAsync<any>(relativeUrl, { d});
		return result;
	}
}

