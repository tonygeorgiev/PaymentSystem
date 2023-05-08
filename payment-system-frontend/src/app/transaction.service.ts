import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transaction } from './models/transaction.model';
@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly baseUrl = 'https://localhost:7208/api/transaction';

  constructor(private http: HttpClient) { }

  getTransactions(): Observable<Transaction[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  getTransactionById(id: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  createTransaction(transaction: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, transaction);
  }

  updateTransaction(id: string, transaction: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/${id}`, transaction);
  }

  deleteTransaction(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }
}
