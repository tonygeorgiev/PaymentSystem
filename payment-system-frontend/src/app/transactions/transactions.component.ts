import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../transaction.service';
import { Transaction } from '../models/transaction.model';
@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {
  transactions: Transaction[] = [];
  displayedColumns = ['id', 'customerEmail', 'customerPhone', 'merchantId', 'amount', 'status', 'transactionType'];

  constructor(private transactionService: TransactionService) { }

  ngOnInit(): void {
    this.fetchTransactions();
  }

  fetchTransactions(): void {
    this.transactionService.getTransactions().subscribe((data: Transaction[]) => {
      this.transactions = data;
    });
  }
}
