import { MyDto }  from "./dto.generated";
import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { format }  from "date-fns";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class RequiredParamTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/required-param-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("RequiredParamTestService instance created");
	}
	explicitStringParam(text: string): Observable<string>  {
		const relativeUrl = `explicit-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { text });
		return result;
	}
	implicitStringParam(text: string): Observable<string>  {
		const relativeUrl = `implicit-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { text });
		return result;
	}
	requiredStringParam(text: string): Observable<string>  {
		const relativeUrl = `required-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { text });
		return result;
	}
	requiredValueType(id: number): Observable<string>  {
		const relativeUrl = `required-value-type`;
		const result = this.doGetStringAsync(relativeUrl, { id });
		return result;
	}
	requiredDateOnly(date: Date): Observable<string>  {
		const relativeUrl = `required-date-only`;
		const result = this.doGetStringAsync(relativeUrl, { date: format(date, "yyyy-MM-dd") });
		return result;
	}
	requiredDateTime(date: Date): Observable<string>  {
		const relativeUrl = `required-datetime`;
		const result = this.doGetStringAsync(relativeUrl, { date: format(date, "yyyy-MM-ddTHH:mm:ssXXX") });
		return result;
	}
	requiredDateTimeAsDateOnly(date: Date): Observable<string>  {
		const relativeUrl = `requried-datetime-as-dateonly`;
		const result = this.doGetStringAsync(relativeUrl, { date: format(date, "yyyy-MM-dd") });
		return result;
	}
	requiredPostParam(dto: MyDto): Observable<object>  {
		const relativeUrl = `required-post-param`;
		const result = this.doPostAsync<object, MyDto>(relativeUrl, dto, {});
		return result;
	}
	requiredStringArray(values: string[]): Observable<string>  {
		const relativeUrl = `required-string-array`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	requiredStringCollection(values: string[]): Observable<string>  {
		const relativeUrl = `required-string-collection`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	requiredValueTypeArray(values: number[]): Observable<string>  {
		const relativeUrl = `required-value-type-array`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	requiredValueTypeCollection(values: number[]): Observable<string>  {
		const relativeUrl = `required-value-type-collection`;
		const result = this.doGetStringAsync(relativeUrl, { values });
		return result;
	}
	requiredDateOnlyCollection(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-date-only-collection`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-dd")) });
		return result;
	}
	requiredDateOnlyArray(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-date-only-array`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-dd")) });
		return result;
	}
	requiredDateTimeCollection(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-datetime-collection`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-ddTHH:mm:ssXXX")) });
		return result;
	}
	requiredDateTimeArray(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-datetime-array`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-ddTHH:mm:ssXXX")) });
		return result;
	}
	requiredDateTimeAsDateOnlyCollection(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-datetime-as-dateonly-collection`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-dd")) });
		return result;
	}
	requiredDateTimeAsDateOnlyArray(dates: Date[]): Observable<string>  {
		const relativeUrl = `required-datetime-as-dateonly-array`;
		const result = this.doGetStringAsync(relativeUrl, { dates: dates.map(x => format(x, "yyyy-MM-dd")) });
		return result;
	}
}
