import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class ArrayParamTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/array-param-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("ArrayParamTestService instance created");
	}
	arrayStringParam(array: string[]): Observable<string>  {
		const relativeUrl = `array-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { a: array });
		return result;
	}
	arrayValueType(array: number[]): Observable<string>  {
		const relativeUrl = `array-value-type`;
		const result = this.doGetStringAsync(relativeUrl, { a: array });
		return result;
	}
	collectionStringParam(collection: string[]): Observable<string>  {
		const relativeUrl = `collection-string-param`;
		const result = this.doGetStringAsync(relativeUrl, { c: collection });
		return result;
	}
	collectionValueType(collection: number[]): Observable<string>  {
		const relativeUrl = `collection-value-type`;
		const result = this.doGetStringAsync(relativeUrl, { c: collection });
		return result;
	}
}
