import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MerchantService } from '../merchant.service';
import { Merchant } from '../models/merchant.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-merchant-edit',
  templateUrl: './merchant-edit.component.html',
  styleUrls: ['./merchant-edit.component.css']
})
export class MerchantEditComponent implements OnInit {

  merchantId!: string | null;
  merchant!: Merchant;
  merchantForm: FormGroup;
  submitted = false;
  subscription!: Subscription;

  constructor(private route: ActivatedRoute, private merchantService: MerchantService, private formBuilder: FormBuilder, private snackBar: MatSnackBar, private location: Location) {
    this.merchantForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      email: ['', [Validators.required, Validators.email]],
      isActive: [true, Validators.required]
    });
  }

  ngOnInit() {
    this.merchantId = this.route.snapshot.paramMap.get('id');
    this.getMerchant();
  }

  getMerchant() {
    this.subscription = this.merchantService.getMerchant(this.merchantId!).subscribe(
      merchant => {
        if (merchant) {
          this.merchant = merchant;
          this.merchantForm.patchValue({
            name: merchant.name,
            description: merchant.description,
            email: merchant.email,
            isActive: merchant.isActive
          });
        }
      },
      error => console.log(error)
    );
  }

  onSubmit() {
    this.submitted = true;

    if (this.merchantForm.invalid) {
      return;
    }

    const merchantToUpdate: Merchant = {
      id: this.merchant.id,
      name: this.merchantForm.value.name,
      description: this.merchantForm.value.description,
      email: this.merchantForm.value.email,
      isActive: this.merchantForm.value.isActive
    };

    this.merchantService.updateMerchant(this.merchant.id, merchantToUpdate).subscribe(
      () => {
        this.snackBar.open('Merchant updated successfully', 'Close', { duration: 3000 });
        this.location.back();
      },
      error => {
        this.snackBar.open('An error occurred while updating the merchant', 'Close', { duration: 3000 });
        console.log(error);
      }
    );
  }

  onCancel() {
    this.location.back();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
