import { OrderItem } from "./OrderItem";

export class Order {
    orderId!: number;
    orderDate: Date = new Date();
    orderNumber!: string;
    items: OrderItem[] = [];

    get subtotal(): number {
        const result = this.items.reduce((tot, val) => {
            return tot + (val.unitPrice * val.quantity);
        }, 0);

        return result; 
    }
}