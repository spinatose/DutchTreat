import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { LoginRequest } from "../shared/LoginRequest";
import { LoginResults } from "../shared/LoginResults";
import { Order } from "../shared/Order";
import { OrderItem } from "../shared/OrderItem";
import { Product } from "../shared/Product";

@Injectable()
export class Store {
    constructor(private http: HttpClient) {
    }

    public products: Product[] = [];
    public order: Order = new Order();
    public token = "";
    public expiration = new Date();

    get loginRequired(): boolean {
        return this.token.length === 0 || this.expiration > new Date();
    }

    checkout() {
        const hdrs = new HttpHeaders().set("Authorization", `Bearer ${this.token}`)
        return this.http.post("/api/orders", this.order, {
            headers: hdrs
        })
            .pipe(map(() => {
                this.order = new Order();
            }));
    }

    loadProducts(): Observable<void> {
        return this.http.get<Product[]>("/api/products")
            .pipe(map(data => {
                this.products = data;
                return;
            }));
    }

    login(creds: LoginRequest) {
        return this.http.post<LoginResults>("/account/createtoken", creds)
            .pipe(map(data => {
                this.token = data.token;
                this.expiration = data.expiration;
            }));
    }

    addToOrder(product: Product) {
        let item : OrderItem | undefined = this.order.items.find(o => o.productId === product.id);

        if (item) {
            item.quantity++;
        } else {
            item = new OrderItem();
            item.productId = product.id;
            item.productTitle = product.title;
            item.productArtId = product.artId;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.unitPrice = product.price;
            item.quantity = 1; 
            this.order.items.push(item);
        }
    }
}