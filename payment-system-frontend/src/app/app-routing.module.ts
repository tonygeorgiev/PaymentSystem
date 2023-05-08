import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MerchantsComponent } from './merchants/merchants.component';
import { MerchantEditComponent } from './merchant-edit/merchant-edit.component';
import { TransactionsComponent } from './transactions/transactions.component';

const routes: Routes = [
  { path: 'merchants', component: MerchantsComponent },
  { path: 'merchants/edit/:id', component: MerchantEditComponent },
  { path: 'transactions', component: TransactionsComponent },
  { path: '', redirectTo: '/merchants', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
