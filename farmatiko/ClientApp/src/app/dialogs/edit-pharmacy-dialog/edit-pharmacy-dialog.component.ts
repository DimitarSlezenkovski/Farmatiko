import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IPharmacy } from '../../shared/interfaces';

@Component({
  selector: 'app-edit-pharmacy-dialog',
  templateUrl: './edit-pharmacy-dialog.component.html',
  styleUrls: ['./edit-pharmacy-dialog.component.css']
})
export class EditPharmacyDialogComponent implements OnInit {
  pharmacy: IPharmacy;

  constructor(private dialogRef: MatDialogRef<EditPharmacyDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.pharmacy = data;
  }

  ngOnInit(): void {
  }

  save() {
    console.log(this.pharmacy);
    this.dialogRef.close(this.pharmacy);
  }

  close() {
    this.dialogRef.close();
  }

}
