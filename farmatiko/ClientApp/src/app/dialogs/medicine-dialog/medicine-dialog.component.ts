import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IMedicine } from '../../shared/interfaces';

@Component({
  selector: 'app-medicine-dialog',
  templateUrl: './medicine-dialog.component.html',
  styleUrls: ['./medicine-dialog.component.css']
})
export class MedicineDialogComponent implements OnInit {
  medicine: IMedicine;

  constructor(private dialogRef: MatDialogRef<MedicineDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.medicine = data;
  }

  ngOnInit(): void {
  }

  close() {
    this.dialogRef.close();
  }

}
