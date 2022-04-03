import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "./store.service";

// will determine which route will be able to activate
@Injectable()
export class SoulGlowActivator implements CanActivate {

    constructor(private store: Store, private router: Router) {

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        if (this.store.loginRequired) {
            this.router.navigate(["login"]);
            return false;
        } else {
            return true;
        }
    }

}