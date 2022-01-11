import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from './data.service';

@Injectable({
	providedIn: 'root'
})
export class MyDataService extends DataService {
	protected endPoint: string;
	constructor(protected client: HttpClient) {
		super()
		this.endPoint = "http://localhost:5000";
	}

	async get(): Promise<string> {
		var value = await this.doGetStringAsync("/api/values", null);
		return value;
	}
}
