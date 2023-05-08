import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-import-csv-dialog',
  templateUrl: './importcsvdialog.component.html',
  styleUrls: ['./importcsvdialog.component.css']
})
export class ImportcsvdialogComponent implements OnInit {

  file!: File;

  constructor(
    private dialogRef: MatDialogRef<ImportcsvdialogComponent>,
    private http: HttpClient
  ) { }

  ngOnInit() {
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length) {
      const file = target.files[0];
      this.file = file;
    }
  }
  

  onUpload() {
    const formData = new FormData();
    formData.append('file', this.file);
    this.http.post('/api/merchants/import', formData)
      .subscribe(() => this.dialogRef.close());
  }

  onCancel() {
    this.dialogRef.close();
  }

}
