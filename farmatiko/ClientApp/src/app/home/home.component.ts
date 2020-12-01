import { Component, OnInit } from '@angular/core';
import { IMedicine, IPharmacy } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { MatDialog } from '@angular/material/dialog';
import { MedicineDialogComponent } from '../dialogs/medicine-dialog/medicine-dialog.component';
import { PharmacyDialogComponent } from '../dialogs/pharmacy-dialog/pharmacy-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public medicines: IMedicine[] = [];
  public pharmacies: IPharmacy[] = [];
  public filteredMedicines: IMedicine[] = [];
  public filteredPharmacies: IPharmacy[] = [];

  constructor(private dataService: DataService, private dialog: MatDialog) {

  }

  ngOnInit(): void {
    this.dataService.getMedicines()
        .subscribe((medicine: IMedicine[]) => {
          this.medicines = this.filteredMedicines = medicine;
        },
        (err: any) => console.log(err),
        () => console.log('Medicine data retrieved'));

    this.dataService.getPharmacies()
        .subscribe((pharmacy: IPharmacy[]) => {
          this.pharmacies = this.filteredPharmacies = pharmacy;
        },
        (err: any) => console.log(err),
        () => console.log('Pharmacy data retrieved'));
  }

  applyFilterMedicines(filterValue: string) {
    console.log("applyFilterMedicines works!")
    if(filterValue) {
      this.dataService.searchMedicines(filterValue)
          .subscribe((medicine: IMedicine[]) => {
            this.filteredMedicines = medicine;
          },
          (err: any) => console.log(err),
          () => console.log('Medicine data retrieved')); 
    }
    else {
      this.filteredMedicines = this.medicines;
    }
  }
  
  applyFilterPharmacies(filterValue: string) {
    console.log("applyFilterPharmacies works!")
    if(filterValue) {
      this.dataService.searchPharmacies(filterValue)
          .subscribe((pharmacy: IPharmacy[]) => {
            this.filteredPharmacies = pharmacy;
          },
          (err: any) => console.log(err),
          () => console.log('Pharmacy data retrieved')); 
    }
    else {
      this.filteredPharmacies = this.pharmacies;
    }
  }

  openMedicineDialog(medicine: IMedicine): void {
    this.dialog.open(MedicineDialogComponent, {
      width: '450px',
      data: medicine
    });
  }

  openPharmacyDialog(pharmacy: IPharmacy): void {
    this.dialog.open(PharmacyDialogComponent, {
      width: '450px',
      data: pharmacy
    });
  }
}
