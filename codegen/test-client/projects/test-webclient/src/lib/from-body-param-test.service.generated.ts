import { MyDto }  from "./dto.generated";
import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class FromBodyParamTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/from-body-param-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("FromBodyParamTestService instance created");
	}
	requiredObject(dto: MyDto): Observable<number>  {
		const relativeUrl = `required-object`;
		const result = this.doPostAsync<number, MyDto>(relativeUrl, dto, {});
		return result;
	}
	nullableObject(dto: MyDto|undefined): Observable<number>  {
		const relativeUrl = `nullable-object`;
		const result = this.doPostAsync<number, MyDto|undefined>(relativeUrl, dto, {});
		return result;
	}
	requiredInt(value: number): Observable<number>  {
		const relativeUrl = `required-int`;
		const result = this.doPostAsync<number, number>(relativeUrl, value, {});
		return result;
	}
	nullableInt(value: number|undefined): Observable<number>  {
		const relativeUrl = `nullable-int`;
		const result = this.doPostAsync<number, number|undefined>(relativeUrl, value, {});
		return result;
	}
	requiredString(value: string): Observable<number>  {
		const relativeUrl = `required-string`;
		const result = this.doPostAsync<number, string>(relativeUrl, value, {});
		return result;
	}
	nullableString(value: string): Observable<number>  {
		const relativeUrl = `nullable-string`;
		const result = this.doPostAsync<number, string>(relativeUrl, value, {});
		return result;
	}
	requiredObjectArray(array: MyDto[]): Observable<number>  {
		const relativeUrl = `required-object-array`;
		const result = this.doPostAsync<number, MyDto[]>(relativeUrl, array, {});
		return result;
	}
	nullableObjectArray(array: (MyDto|undefined)[]): Observable<number>  {
		const relativeUrl = `nullable-object-array`;
		const result = this.doPostAsync<number, (MyDto|undefined)[]>(relativeUrl, array, {});
		return result;
	}
}
