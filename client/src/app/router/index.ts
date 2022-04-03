import { RouterModule } from "@angular/router";
import { Checkout } from "../pages/checkout.component";
import { LoginPage } from "../pages/loginPage.component";
import { ShopPage } from "../pages/shopPage.component";
import { SoulGlowActivator } from "../services/authActivator.service";

const routes = [
    { path: "", component: ShopPage },
    { path: "checkout", component: Checkout, canActivate: [SoulGlowActivator] },
    { path: "login", component: LoginPage },
    { path: "**", redirectTo: "/" },
];

const router = RouterModule.forRoot(routes, { useHash: false });

export default router;