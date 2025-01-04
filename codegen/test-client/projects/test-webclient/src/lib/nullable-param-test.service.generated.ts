import { MyDto }  from "./dto.generated";
import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class NullableParamTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/nullable-param-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("NullableParamTestService instance created");
	}
	nullableStringParam(text: string): Observable<string>  {
		const relativeUrl = `nullable-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { text });
		return result;
	}
	nullableValueType(id: number|undefined): Observable<string>  {
		const relativeUrl = `nullable-value-type`;
		const result = this.doGetStringAsync(relativeUrl, { id });
		return result;
	}
	nullableDateOnly(date: Date|undefined): Observable<string>  {
		const relativeUrl = `nullable-date-only`;
		const result = this.doGetStringAsync(relativeUrl, { date });
		return result;
	}
	nullablePostParam(dto: MyDto|undefined): Observable<object>  {
		const relativeUrl = `nullable-post-param`;
		const result = this.doPostAsync<object, MyDto|undefined>(relativeUrl, dto, {});
		return result;
	}
	nullableStringArray(values: string[]): Observable<string>  {
		const relativeUrl = `nullable-string-array`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	nullableStringCollection(values: string[]): Observable<string>  {
		const relativeUrl = `nullable-string-collection`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	nullableValueTypeArray(values: (number|undefined)[]): Observable<string>  {
		const relativeUrl = `nullable-value-type-array`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	nullableValueTypeCollection(values: (number|undefined)[]): Observable<string>  {
		const relativeUrl = `nullable-value-type-collection`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	nullableDateOnlyCollection(dates: (Date|undefined)[]): Observable<string>  {
		const relativeUrl = `nullable-date-only-collection`;
		const result = this.doGetStringAsync(relativeUrl, { dates });
		return result;
	}
	nullableDateOnlyArray(dates: (Date|undefined)[]): Observable<string>  {
		const relativeUrl = `nullable-date-only-array`;
		const result = this.doGetStringAsync(relativeUrl, { dates });
		return result;
	}
}
