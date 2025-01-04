import { MyDto }  from "./dto.generated";
import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class RequiredReturnTypeTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/required-return-type";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("RequiredReturnTypeTestService instance created");
	}
	get(): Observable<object>  {
		const relativeUrl = `void`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	getAsync(): Observable<object>  {
		const relativeUrl = `async-task`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	getActionResult(): Observable<object>  {
		const relativeUrl = `action-result`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	getAsyncActionResult(): Observable<object>  {
		const relativeUrl = `async-action-result`;
		const result = this.doGetAsync<object>(relativeUrl, {});
		return result;
	}
	getString(): Observable<string>  {
		const relativeUrl = `string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getAsyncString(): Observable<string>  {
		const relativeUrl = `async-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getActionResultString(): Observable<string>  {
		const relativeUrl = `action-result-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getAsyncActionResultString(): Observable<string>  {
		const relativeUrl = `async-action-result-string`;
		const result = this.doGetStringAsync(relativeUrl, {});
		return result;
	}
	getInt(): Observable<number>  {
		const relativeUrl = `int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getAsyncInt(): Observable<number>  {
		const relativeUrl = `async-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getActionResultInt(): Observable<number>  {
		const relativeUrl = `action-result-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultInt(): Observable<number>  {
		const relativeUrl = `async-action-result-int`;
		const result = this.doGetAsync<number>(relativeUrl, {});
		return result;
	}
	getDateTime(): Observable<Date>  {
		const relativeUrl = `datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getAsyncDateTime(): Observable<Date>  {
		const relativeUrl = `async-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getActionResultDateTime(): Observable<Date>  {
		const relativeUrl = `action-result-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultDateTime(): Observable<Date>  {
		const relativeUrl = `async-action-result-datetime`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getDateOnly(): Observable<Date>  {
		const relativeUrl = `dateonly`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getDateTimeOffset(): Observable<Date>  {
		const relativeUrl = `datetimeoffset`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getTimeOnly(): Observable<Date>  {
		const relativeUrl = `timeonly`;
		const result = this.doGetAsync<Date>(relativeUrl, {});
		return result;
	}
	getMyDto(): Observable<MyDto>  {
		const relativeUrl = `object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	getAsyncMyDto(): Observable<MyDto>  {
		const relativeUrl = `async-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	actionResultObject(): Observable<MyDto>  {
		const relativeUrl = `action-result-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	asyncActionResultObject(): Observable<MyDto>  {
		const relativeUrl = `async-action-result-object`;
		const result = this.doGetAsync<MyDto>(relativeUrl, {});
		return result;
	}
	getMyDtoArray(): Observable<MyDto[]>  {
		const relativeUrl = `array-return-type`;
		const result = this.doGetAsync<MyDto[]>(relativeUrl, {});
		return result;
	}
	getMyDtoCollection(): Observable<MyDto[]>  {
		const relativeUrl = `collection-return-type`;
		const result = this.doGetAsync<MyDto[]>(relativeUrl, {});
		return result;
	}
	getMyDtoCollectionAsync(): Observable<MyDto[]>  {
		const relativeUrl = `async-collection-return-type`;
		const result = this.doGetAsync<MyDto[]>(relativeUrl, {});
		return result;
	}
}
