export interface Transaction {
    amount: number;
    status: number;
    customerEmail: string;
    customerPhone: string;
    merchantId: string;
    merchant: any;
    transactionType: number;
    referencedTransactionId: string;
    referencedTransaction: any;
    id: string;
  }
  