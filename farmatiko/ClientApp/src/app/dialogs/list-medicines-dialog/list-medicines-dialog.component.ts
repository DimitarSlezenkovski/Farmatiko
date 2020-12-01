import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { IMedicine } from '../../shared';
import { DataService } from '../../shared/data.service';

@Component({
  selector: 'app-list-medicines-dialog-component',
  templateUrl: './list-medicines-dialog.component.html',
  styleUrls: ['./list-medicines-dialog.component.css']
})
export class ListMedicinesDialogComponent implements OnInit {
  medicines: IMedicine[] = [];
  listMedicines: IMedicine[];
  filteredListMedicines: IMedicine[];

  constructor(private dialogRef: MatDialogRef<ListMedicinesDialogComponent>, private dataService: DataService) {
  }

  ngOnInit(): void {
    this.dataService.getMedicines()
        .subscribe((medicine: IMedicine[]) => {
          this.listMedicines = this.filteredListMedicines = medicine;
        },
        (err: any) => console.log(err),
        () => console.log('Medicine data retrieved'));
  }

  selectMedicine(selectedMedicine: IMedicine) {
    if(this.medicines.filter(x => x == selectedMedicine)) {
      this.medicines = this.medicines.filter(x => x != selectedMedicine);
    }
    this.medicines.push(selectedMedicine);
  }

  save() {
    console.log(this.medicines);
    this.dialogRef.close(this.medicines);
  }

  close() {
    this.dialogRef.close();
  }

  applyFilterMedicines(filterValue: string) {
    if(filterValue) {
      this.dataService.searchMedicines(filterValue)
          .subscribe((medicine: IMedicine[]) => {
            this.filteredListMedicines = medicine;
          },
          (err: any) => console.log(err),
          () => console.log('Medicine data retrieved')); 
    }
    else {
      this.filteredListMedicines = this.listMedicines;
    }
  }

}
