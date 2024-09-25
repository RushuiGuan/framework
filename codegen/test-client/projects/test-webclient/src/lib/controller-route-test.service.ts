import { HttpClient }  from '@angular/common/http';
import { Injectable }  from '@angular/core';
import { ConfigService, DataService, Logger }  from 'welton-core';

@Injectable({"providedIn":"root"})
export class ControllerRouteTestService extends DataService {
	get endPoint():string {
		return this.config.endpoint('test') + 'api/controllerroutetest/'
	}
	constructor(private config: ConfigService, protected client: HttpClient, logger: Logger) {
		super(logger);
		this.logger.info("ControllerRouteTestService instance created");
	}
}

