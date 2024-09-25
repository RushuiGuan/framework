import { MyDto }  from './dto';
import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class FromBodyParamTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/from-body-param-test/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("FromBodyParamTestService instance created");
	}
	async requiredObject(dto: MyDto): Promise<any>  {
		const relativeUrl = `required-object`;
		const result = await this.doPostAsync<any,MyDto>(relativeUrl, dto, null);
		return result;
	}
	async nullableObject(dto: MyDto): Promise<any>  {
		const relativeUrl = `nullable-object`;
		const result = await this.doPostAsync<any,MyDto>(relativeUrl, dto, null);
		return result;
	}
	async requiredInt(value: number): Promise<any>  {
		const relativeUrl = `required-int`;
		const result = await this.doPostAsync<any,number>(relativeUrl, value, null);
		return result;
	}
	async nullableInt(value: number): Promise<any>  {
		const relativeUrl = `nullable-int`;
		const result = await this.doPostAsync<any,number>(relativeUrl, value, null);
		return result;
	}
	async requiredString(value: string): Promise<any>  {
		const relativeUrl = `required-string`;
		const result = await this.doPostAsync<any,string>(relativeUrl, value, null);
		return result;
	}
	async nullableString(value: string): Promise<any>  {
		const relativeUrl = `nullable-string`;
		const result = await this.doPostAsync<any,string>(relativeUrl, value, null);
		return result;
	}
	async requiredObjectArray(array: MyDto[]): Promise<any>  {
		const relativeUrl = `required-object-array`;
		const result = await this.doPostAsync<any,MyDto[]>(relativeUrl, array, null);
		return result;
	}
	async nullableObjectArray(array: MyDto[]): Promise<any>  {
		const relativeUrl = `nullable-object-array`;
		const result = await this.doPostAsync<any,MyDto[]>(relativeUrl, array, null);
		return result;
	}
}

