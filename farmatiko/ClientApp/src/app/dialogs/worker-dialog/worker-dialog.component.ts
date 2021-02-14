import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IHealthcareWorkers } from '../../shared/interfaces';

@Component({
  selector: 'app-worker-dialog',
  templateUrl: './worker-dialog.component.html',
  styleUrls: ['./worker-dialog.component.css']
})
export class WorkerDialogComponent implements OnInit {
  worker: IHealthcareWorkers;
  isExpanded: boolean = false;

  constructor(private dialogRef: MatDialogRef<WorkerDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
    this.worker = data;
  }

  ngOnInit(): void {
  }

  close() {
    this.dialogRef.close();
  }

  toggleExpansion() {
    this.isExpanded = !this.isExpanded;
  }

}