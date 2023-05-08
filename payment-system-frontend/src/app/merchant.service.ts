import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Merchant } from './models/merchant.model';

@Injectable({
  providedIn: 'root'
})
export class MerchantService {
  private apiUrl = 'https://localhost:7208/api';

  constructor(private http: HttpClient) { }

  getMerchants(): Observable<any> {
    return this.http.get(`${this.apiUrl}/merchant`);
  }

  getMerchant(merchantId: string): Observable<Merchant> {
    return this.http.get<Merchant>(`${this.apiUrl}/merchant/${merchantId}`);
  }

  updateMerchant(merchantId: string, merchant: Merchant): Observable<Merchant> {
    return this.http.put<Merchant>(`${this.apiUrl}/merchant/${merchantId}`, merchant);
  }

  deleteMerchant(merchantId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/merchant/${merchantId}`);
  }

}
