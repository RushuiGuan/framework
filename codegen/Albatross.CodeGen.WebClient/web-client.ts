import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Logger } from '@mirage/logging';

export abstract class WebClient {
	protected abstract get endPoint(): string;
	protected abstract client: HttpClient;
	protected useWindowsCredential = true;

	constructor(protected logger: Logger) { }

	public getPath(route: string, query: any) {
		if (!route) { route = ""; }
		const url = `${this.endPoint}${route}`;
		if (query) {
			const params = new URLSearchParams();
			for (const key of Object.keys(query)) {
				if (query[key] !== null && query[key] !== undefined) {
					if (query[key] instanceof Array) {
						for (const item of query[key]) {
							if (item !== null && item !== undefined) {
								params.append(key, item);
							}
						}
					} else {
						params.set(key, query[key]);
					}
				}
			}
			return `${url}?${params.toString()}`;
		} else {
			return url;
		}
	}
	public async doGetBlobAsync(route: string, query: any = null) {
		const path = this.getPath(route, query);
		this.logger.info(`get blob:${path}`);
		return await firstValueFrom(this.client.get(path, { responseType: 'blob', withCredentials: this.useWindowsCredential }));
	}
	public async doGetAsync<TResponse>(
		route: string,
		query: any = null
	): Promise<TResponse> {
		const path = this.getPath(route, query);
		this.logger.info(`get:${path}`);
		return await firstValueFrom(this.client.get<TResponse>(path, { withCredentials: this.useWindowsCredential }));
	}
	public async doGetStringAsync(
		route: string,
		query: any = null,
	): Promise<string> {
		const path = this.getPath(route, query);
		this.logger.info(`get:${path}`);
		return await firstValueFrom(this.client.get(path, { responseType: 'text', withCredentials: this.useWindowsCredential }));

	}
	public async doPostAsync<TResponse, TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		this.logger.info(`post:${path}`);
		return await firstValueFrom(this.client.post<TResponse>(path, request, { withCredentials: this.useWindowsCredential }));
	}
	public async doPostStringAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	): Promise<string> {
		const path = this.getPath(route, query);
		this.logger.info(`post:${path}`);
		return await firstValueFrom(this.client.post(path, request, { responseType: 'text', withCredentials: this.useWindowsCredential }));
	}
	public async doPostBlobAsync<TResponse>(
		route: string,
		blob: File | Blob,
		query: any = null
	): Promise<TResponse> {
		const formData = new FormData();
		if (blob instanceof File) {
			formData.append(blob.name, blob);
		}
		const path = this.getPath(route, query);
		this.logger.info(`post blob:${path}`);
		return await firstValueFrom(this.client.post<TResponse>(path, formData, { withCredentials: this.useWindowsCredential, }));
	}
	public async doPostMultiBlobAsync<TResponse>(
		route: string,
		files: File[] | Blob[],
		query: any = null
	): Promise<TResponse> {
		const formData = new FormData();
		files.forEach((file) => {
			if (file instanceof File) {
				formData.append(file.name, file);
			}
		});
		const path = this.getPath(route, query);
		this.logger.info(`post blob:${path}`);
		return await firstValueFrom(this.client.post<TResponse>(path, formData, { withCredentials: this.useWindowsCredential, }));
	}
	public async doPatchAsync<TResponse, TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		this.logger.info(`patch:${path}`);
		return await firstValueFrom(this.client.patch<TResponse>(path, request, { withCredentials: this.useWindowsCredential }));
	}
	public async doPatchStringAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		this.logger.info(`patch:${path}`);
		return firstValueFrom(await this.client.patch(path, request, { responseType: 'text', withCredentials: this.useWindowsCredential }));
	}
	public async doPutAsync<TRequest>(
		route: string,
		request: TRequest,
		query: any = null
	) {
		const path = this.getPath(route, query);
		this.logger.info(`put:${path}`);
		return await firstValueFrom(this.client.put(path, request, { withCredentials: this.useWindowsCredential }));
	}
	public async doDeleteAsync(
		route: string,
		query: any = null
	): Promise<any> {
		const path = this.getPath(route, query);
		this.logger.info(`delete:${path}`);
		return await firstValueFrom(this.client.delete(path, { withCredentials: this.useWindowsCredential }));
	}
	public async doDownload(route: string, query: any, filename: string) {
		const blob = await this.doGetBlobAsync(route, query);
		const downloadUrl = window.URL.createObjectURL(blob);
		const link = document.createElement('a');
		link.href = downloadUrl;
		link.download = filename;
		link.click();
	}
}
