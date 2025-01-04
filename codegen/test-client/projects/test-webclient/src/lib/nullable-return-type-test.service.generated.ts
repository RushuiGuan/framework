import { MyDto }  from "./dto.generated";
import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
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
	getInt(): Observable<number|undefined>  {
		const relativeUrl = `int`;
		const result = this.doGetAsync<number|undefined>(relativeUrl, {});
		return result;
	}
	getAsyncInt(): Observable<number|undefined>  {
		const relativeUrl = `async-int`;
		const result = this.doGetAsync<number|undefined>(relativeUrl, {});
		return result;
	}
	getActionResultInt(): Observable<number|undefined>  {
		const relativeUrl = `action-result-int`;
		const result = this.doGetAsync<number|undefined>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultInt(): Observable<number|undefined>  {
		const relativeUrl = `async-action-result-int`;
		const result = this.doGetAsync<number|undefined>(relativeUrl, {});
		return result;
	}
	getDateTime(): Observable<Date|undefined>  {
		const relativeUrl = `datetime`;
		const result = this.doGetAsync<Date|undefined>(relativeUrl, {});
		return result;
	}
	getAsyncDateTime(): Observable<Date|undefined>  {
		const relativeUrl = `async-datetime`;
		const result = this.doGetAsync<Date|undefined>(relativeUrl, {});
		return result;
	}
	getActionResultDateTime(): Observable<Date|undefined>  {
		const relativeUrl = `action-result-datetime`;
		const result = this.doGetAsync<Date|undefined>(relativeUrl, {});
		return result;
	}
	getAsyncActionResultDateTime(): Observable<Date|undefined>  {
		const relativeUrl = `async-action-result-datetime`;
		const result = this.doGetAsync<Date|undefined>(relativeUrl, {});
		return result;
	}
	getMyDto(): Observable<MyDto|undefined>  {
		const relativeUrl = `object`;
		const result = this.doGetAsync<MyDto|undefined>(relativeUrl, {});
		return result;
	}
	getAsyncMyDto(): Observable<MyDto|undefined>  {
		const relativeUrl = `async-object`;
		const result = this.doGetAsync<MyDto|undefined>(relativeUrl, {});
		return result;
	}
	actionResultObject(): Observable<MyDto|undefined>  {
		const relativeUrl = `action-result-object`;
		const result = this.doGetAsync<MyDto|undefined>(relativeUrl, {});
		return result;
	}
	asyncActionResultObject(): Observable<MyDto|undefined>  {
		const relativeUrl = `async-action-result-object`;
		const result = this.doGetAsync<MyDto|undefined>(relativeUrl, {});
		return result;
	}
	getMyDtoNullableArray(): Observable<(MyDto|undefined)[]>  {
		const relativeUrl = `nullable-array-return-type`;
		const result = this.doGetAsync<(MyDto|undefined)[]>(relativeUrl, {});
		return result;
	}
	getMyDtoCollection(): Observable<(MyDto|undefined)[]>  {
		const relativeUrl = `nullable-collection-return-type`;
		const result = this.doGetAsync<(MyDto|undefined)[]>(relativeUrl, {});
		return result;
	}
}
