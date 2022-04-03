import { Component, OnInit } from "@angular/core";
import { Store } from "../services/store.service";

@Component({
    selector: "product-list",
    templateUrl: "productListView.component.html",
    styleUrls: [ "productListView.component.css"]
})

export class ProductListView implements OnInit {

    constructor(public store: Store) {
    }

    ngOnInit(): void {
        this.store.loadProducts().subscribe(); // subscribe starts invokes the observable representing the http get
    }
}