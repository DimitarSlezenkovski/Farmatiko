import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IPharmacy } from '../../shared/interfaces';

@Component({
  selector: 'app-pharmacy-dialog',
  templateUrl: './pharmacy-dialog.component.html',
  styleUrls: ['./pharmacy-dialog.component.css']
})
export class PharmacyDialogComponent implements OnInit {
  pharmacy: IPharmacy;

  constructor(private dialogRef: MatDialogRef<PharmacyDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.pharmacy = data;
  }

  ngOnInit(): void {
  }

  close() {
    this.dialogRef.close();
  }

}