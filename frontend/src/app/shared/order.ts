import { Customer } from 'src/app/shared/customer';


export interface Order{
    id: number,
    customer: Customer,
    total: number,
    placed: Date,
    completed: Date
}