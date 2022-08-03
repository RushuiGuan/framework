import { Component, OnInit } from '@angular/core';
import { MyDataService } from '../my-data.service'

@Component({
	selector: 'app-page1',
	templateUrl: './page1.component.html',
	styleUrls: ['./page1.component.css']
})
export class Page1Component implements OnInit {
	data: string;
	constructor(private svc: MyDataService) { }

	ngOnInit(): void {
	}

	async Run1() {
		this.data = await this.svc.get();
	}
}
