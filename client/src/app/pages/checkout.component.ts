import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";
//import { DataService } from '../shared/dataService';

@Component({
  selector: "checkout",
  templateUrl: "checkout.component.html",
  styleUrls: ['checkout.component.css']
})
export class Checkout {

    constructor(
        public store: Store,
        private router: Router
    ) {
  }

    public errMessage: string = "";

    onCheckout() {
        this.errMessage = "";
        this.store.checkout()
            .subscribe(() => {
                this.router.navigate(["/"]);
            }, err => {
                this.errMessage = `failed to checkout: ${err.message}`; 
            });
  }
}