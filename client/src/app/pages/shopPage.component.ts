import { Component } from "@angular/core";

@Component({
    selector: "shop-page",
    templateUrl: "shopPage.component.html",
    styleUrls: ["shopPage.component.css"]
})

export class ShopPage {
    title: string = 'Shop HERE!';
    constructor() {
    }
}