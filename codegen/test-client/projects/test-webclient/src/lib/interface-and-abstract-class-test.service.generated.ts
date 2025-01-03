import { HttpClient }  from "@angular/common/http";
import { Injectable }  from "@angular/core";
import { ConfigService }  from "@mirage/config";
import { WebClient }  from "@mirage/webclient";
import { AbstractClass, ICommand }  from './dto.generated';
import { Observable }  from "rxjs";

@Injectable({ providedIn: "root" })
export class InterfaceAndAbstractClassTestService extends WebClient {
	get endPoint(): string  {
		return this.config.endpoint("test-client") + "api/interface-abstract-class-test";
	}
	constructor(private config: ConfigService, protected client: HttpClient) {
		super();
		console.log("InterfaceAndAbstractClassTestService instance created");
	}
	submitByInterface(command: ICommand): Observable<object>  {
		const relativeUrl = `interface-as-param`;
		const result = this.doPostAsync<object, string>(relativeUrl, "", { command });
		return result;
	}
	submitByAbstractClass(command: AbstractClass): Observable<object>  {
		const relativeUrl = `abstract-class-as-param`;
		const result = this.doPostAsync<object, string>(relativeUrl, "", { command });
		return result;
	}
	returnInterfaceAsync(): Observable<ICommand>  {
		const relativeUrl = `return-interface-async`;
		const result = this.doPostAsync<ICommand, string>(relativeUrl, "", {});
		return result;
	}
	returnInterface(): Observable<ICommand>  {
		const relativeUrl = `return-interface`;
		const result = this.doPostAsync<ICommand, string>(relativeUrl, "", {});
		return result;
	}
	returnAbstractClassAsync(): Observable<AbstractClass>  {
		const relativeUrl = `return-abstract-class-async`;
		const result = this.doPostAsync<AbstractClass, string>(relativeUrl, "", {});
		return result;
	}
	returnAbstractClass(): Observable<AbstractClass>  {
		const relativeUrl = `return-abstract-class`;
		const result = this.doPostAsync<AbstractClass, string>(relativeUrl, "", {});
		return result;
	}
}
