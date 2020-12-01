import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IPharmacyHead } from '../../shared/interfaces';

@Component({
  selector: 'app-edit-pharmacy-head-dialog',
  templateUrl: './edit-pharmacy-head-dialog.component.html',
  styleUrls: ['./edit-pharmacy-head-dialog.component.css']
})
export class EditPharmacyHeadDialogComponent implements OnInit {
  head: IPharmacyHead;

  constructor(private dialogRef: MatDialogRef<EditPharmacyHeadDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.head = data;
  }

  ngOnInit(): void {
  }

  save() {
    this.dialogRef.close(this.head);
  }

  close() {
    this.dialogRef.close();
  }

}