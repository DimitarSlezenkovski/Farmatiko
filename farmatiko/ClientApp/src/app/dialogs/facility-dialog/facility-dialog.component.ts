import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IHealthFacilities } from '../../shared/interfaces';

@Component({
  selector: 'app-facility-dialog',
  templateUrl: './facility-dialog.component.html',
  styleUrls: ['./facility-dialog.component.css']
})
export class FacilityDialogComponent implements OnInit {
  facility: IHealthFacilities;

  constructor(private dialogRef: MatDialogRef<FacilityDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.facility = data;
  }

  ngOnInit(): void {
  }

  close() {
    this.dialogRef.close();
  }

}
