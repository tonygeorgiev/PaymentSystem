import { Component, OnInit } from '@angular/core';
import { MerchantService } from '../merchant.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { ImportcsvdialogComponent } from '../importcsvdialog/importcsvdialog.component';
@Component({
  selector: 'app-merchants',
  templateUrl: './merchants.component.html',
  styleUrls: ['./merchants.component.css']
})
export class MerchantsComponent implements OnInit {
  merchants: any[] = [];
  displayedColumns: string[] = ['id', 'name', 'description', 'email', 'isActive', 'actions'];
  selectedFile: File | null = null;
  constructor(private merchantService: MerchantService, private router: Router, private dialog: MatDialog, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.fetchMerchants();
  }

  fetchMerchants(): void {
    this.merchantService.getMerchants().subscribe((data: any) => {
      this.merchants = data;
    });
  }

  onEditMerchant(merchantId: string): void {
    // Navigate to the edit page, passing the merchant ID
    this.router.navigate(['/merchants/edit', merchantId]);
  }
  
  async onDeleteMerchant(merchantId: string): Promise<void> {
    // Show a confirmation dialog before deleting the merchant
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Merchant',
        message: 'Are you sure you want to delete this merchant?'
      }
    });
  
    dialogRef.afterClosed().subscribe(async (confirmed: boolean) => {
      if (confirmed) {
        try {
          await this.merchantService.deleteMerchant(merchantId).toPromise();
          this.snackBar.open('Merchant deleted successfully', 'Close', { duration: 3000 });
          this.fetchMerchants();
        } catch (error) {
          this.snackBar.open('Error deleting merchant', 'Close', { duration: 3000 });
        }
      }
    });
  }


  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    const formData = new FormData();
    formData.append('file', this.selectedFile!, this.selectedFile!.name);

    const dialogRef = this.dialog.open(ImportcsvdialogComponent, {
      width: '500px',
      data: { formData: formData },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.merchantService.importMerchantsCSV(formData).subscribe(() => {
          this.fetchMerchants();
        });
      }
    });
  }
  
}
