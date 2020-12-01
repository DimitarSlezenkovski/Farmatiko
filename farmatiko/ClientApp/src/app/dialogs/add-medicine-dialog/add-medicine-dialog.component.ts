import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { IMedicine } from '../../shared';
import { FormGroup, Validators, FormBuilder }  from '@angular/forms';

@Component({
  selector: 'app-add-medicine-dialog',
  templateUrl: './add-medicine-dialog.component.html',
  styleUrls: ['./add-medicine-dialog.component.css']
})
export class AddMedicineDialogComponent implements OnInit {
  medicine: IMedicine;
  forma: FormGroup;

  constructor(private dialogRef: MatDialogRef<AddMedicineDialogComponent>, private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.forma = this.formBuilder.group({
      name: ['', [Validators.required]],
      strength: ['', [Validators.required]],
      form: ['', [Validators.required]],
      wayOfIssuing: ['', [Validators.required]],
      manufacturer: ['', [Validators.required]],
      price: ['', [Validators.required, Validators.min(0)]],
      packaging: ['', [Validators.required]]
    });
  }

  save() {
    this.medicine = this.forma.value;
    console.log(this.forma.value);
    console.log(this.medicine);
    this.forma.reset();
    this.dialogRef.close(this.medicine);
  }

  close() {
    this.dialogRef.close();
  }
}
