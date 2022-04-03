import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";
import { LoginRequest } from "../shared/LoginRequest";

@Component({
    selector: "login-page",
    templateUrl: "loginPage.component.html",
    styleUrls: ["loginPage.component.css"]
})
export class LoginPage {
    constructor(private store: Store, private router: Router) {

    }

    public creds: LoginRequest = {
        username: "",
        password: ""
    }

    public errMessage: string = "";

    onLogin() {
        this.store.login(this.creds)
            .subscribe(() => {
                //success
                if (this.store.order.items.length > 0)
                    this.router.navigate(["checkout"]);
                else
                    this.router.navigate([""]);
            }, error => {
                this.errMessage = "unable to login- bad username or password";
                console.log(error);
            });
    }
}
