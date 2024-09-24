import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { MyDto }  from './dto.generated';
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class NullableReturnTypeTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/nullable-return-type";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("NullableReturnTypeTestService instance created");
	}
	getString(): Observable<string>  {
		const relativeUrl = `/string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getAsyncString(): Observable<string>  {
		const relativeUrl = `/async-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getActionResultString(): Observable<string>  {
		const relativeUrl = `/action-result-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getAsyncActionResultString(): Observable<string>  {
		const relativeUrl = `/async-action-result-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getInt(): Observable<number>  {
		const relativeUrl = `/int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getAsyncInt(): Observable<number>  {
		const relativeUrl = `/async-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getActionResultInt(): Observable<number>  {
		const relativeUrl = `/action-result-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultInt(): Observable<number>  {
		const relativeUrl = `/async-action-result-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getDateTime(): Observable<Date>  {
		const relativeUrl = `/datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getAsyncDateTime(): Observable<Date>  {
		const relativeUrl = `/async-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getActionResultDateTime(): Observable<Date>  {
		const relativeUrl = `/action-result-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultDateTime(): Observable<Date>  {
		const relativeUrl = `/async-action-result-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getMyDto(): Observable<MyDto>  {
		const relativeUrl = `/object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	getAsyncMyDto(): Observable<MyDto>  {
		const relativeUrl = `/async-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	actionResultObject(): Observable<MyDto>  {
		const relativeUrl = `/action-result-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	asyncActionResultObject(): Observable<MyDto>  {
		const relativeUrl = `/async-action-result-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	getMyDtoNullableArray(): Observable<MyDto[]>  {
		const relativeUrl = `/nullable-array-return-type`;
		const result = this.doGetAsync<MyDto[]>(relativeUrl, {});
		return result;
	}
	getMyDtoCollection(): Observable<MyDto[]>  {
		const relativeUrl = `/nullable-collection-return-type`;
		const result = this.doGetAsync<MyDto[]>(relativeUrl, {});
		return result;
	}
}
