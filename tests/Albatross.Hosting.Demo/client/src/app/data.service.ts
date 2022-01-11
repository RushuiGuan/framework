import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from './config.service';

export abstract class DataService {
	protected abstract endPoint: string;
	protected abstract client: HttpClient;
	private useWindowsCredential = false;

	constructor() {
		this.useWindowsCredential = true;
	}

	protected getPath(route: string, query: any) {
		if (!route) { route = ""; }
		const url = `${this.endPoint}${route}`;
		if (query) {
			const params = new URLSearchParams();
			for (const key in query) {
				if (query.hasOwnProperty(key) && query[key] !== null && query[key] !== undefined) {
					params.set(key, query[key]);
				}
			}
			return `${url}?${params.toString()}`;
		} else {
			return url;
		}
	}

	protected async doGetBlobAsync(route: string, query: any = null) {
		const path = this.getPath(route, query);
		console.log(`data service-get blob:${path}`);
		return await this.client
			.get(path, { responseType: 'blob', withCredentials: this.useWindowsCredential })
			.toPromise();
	}

	protected async doGetAsync<TResponse>(
		route: string,
		query: any = null
	): Promise<TResponse> {
		const path = this.getPath(route, query);
		console.log(`data service-get:${path}`, this.useWindowsCredential);
		return await this.client
			.get<TResponse>(path, { withCredentials: this.useWindowsCredential })
			.toPromise<TResponse>();
	}

	protected async doGetStringAsync(
		route: string,
		query: any = null,
	): Promise<string> {
		const path = this.getPath(route, query);
		console.log(`data service-get:${path}`, this.useWindowsCredential);
		return await this.client
			.get(path, { responseType: 'text', withCredentials: this.useWindowsCredential })
			.toPromise<string>();
	}

	protected async doPostAsync<TResponse, TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		console.log(`data service-post:${path}`, this.useWindowsCredential);
		return await this.client
			.post<TResponse>(path, request, { withCredentials: this.useWindowsCredential })
			.toPromise<TResponse>();
	}

	protected async doPostStringAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	): Promise<string> {
		const path = this.getPath(route, query);
		console.log(`data service-post:${path}`, this.useWindowsCredential);
		return await this.client
			.post(path, request, { responseType: 'text', withCredentials: this.useWindowsCredential })
			.toPromise<string>();
	}

	protected async doPostBlobAsync<TResponse>(
		route: string,
		blob: File,
		query: any = null
	): Promise<TResponse> {
		const formData = new FormData();
		formData.append(blob.name, blob);
		const path = this.getPath(route, query);
		console.log(`data service-post blob:${path}`, this.useWindowsCredential);
		return await this.client
			.post<TResponse>(path, formData, {
				withCredentials: this.useWindowsCredential,
			})
			.toPromise<TResponse>();
	}


	protected async doPatchAsync<TResponse, TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		console.log(`data service-patch:${path}`, this.useWindowsCredential);
		return await this.client
			.patch<TResponse>(path, request, { withCredentials: this.useWindowsCredential })
			.toPromise<TResponse>();
	}
	protected async doPatchStringAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		console.log(`data service-patch:${path}`, this.useWindowsCredential);
		return await this.client
			.patch(path, request, { responseType: 'text', withCredentials: this.useWindowsCredential })
			.toPromise<string>();
	}

	protected async doPutAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		console.log(`data service-put:${path}`, this.useWindowsCredential);
		return await this.client
			.put(path, request, { withCredentials: this.useWindowsCredential })
			.toPromise();
	}

	protected async doDeleteAsync(
		route: string,
		query: any = null
	): Promise<any> {
		const path = this.getPath(route, query);
		console.log(`data service-delete:${path}`, this.useWindowsCredential);
		return await this.client
			.delete(path, { withCredentials: this.useWindowsCredential })
			.toPromise();
	}

	protected async doDownload(route: string, query: any, filename: string) {
		const blob = await this.doGetBlobAsync(route, query);
		const downloadUrl = window.URL.createObjectURL(blob);
		const link = document.createElement('a');
		link.href = downloadUrl;
		link.download = filename;
		link.click();
	}
}
