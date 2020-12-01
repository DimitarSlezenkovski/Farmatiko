import { Component, OnInit} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material/snack-bar';
import { IPharmacy, IMedicine, IPharmacyHead, IPharmacyHeadRequest } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { PharmacyDialogComponent } from '../dialogs/pharmacy-dialog/pharmacy-dialog.component';
import { EditPharmacyDialogComponent } from '../dialogs/edit-pharmacy-dialog/edit-pharmacy-dialog.component';
import { MedicineDialogComponent } from '../dialogs/medicine-dialog/medicine-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { AddMedicineDialogComponent } from '../dialogs/add-medicine-dialog/add-medicine-dialog.component';
import { ListMedicinesDialogComponent } from '../dialogs/list-medicines-dialog/list-medicines-dialog.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public pharmacies: IPharmacy[] = [];
  public head: IPharmacyHead;
  public filteredPharmacies: IPharmacy[] = [];
  public filteredMedicines: IMedicine[] = [];
  public request: IPharmacyHeadRequest;
  editedMedicine: boolean = false;
  medicinesEditMode: boolean = false;

  constructor(private dataService: DataService, private authService: AuthService, private dialog: MatDialog, private snackBar: MatSnackBar, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.authService.getUser()
        .subscribe((data) => {
          console.log(data);
          this.head = data;
        },
        (err: any) => console.log(err),
        () => console.log('User data retrieved'));
    this.dataService.getPharmacies()
        .subscribe((pharmacy: IPharmacy[]) => {
          this.pharmacies = pharmacy;
          this.head.Pharmacy.forEach((pharma) => {
            this.filteredPharmacies = this.pharmacies = this.pharmacies.filter(x => x == pharma);
          });
        },
        (err: any) => console.log(err),
        () => console.log('Pharmacy data retrieved'));
    this.filteredMedicines = this.head.PharmacyMedicines;
  }
  
  claimPharmacy(pharmacy: IPharmacy) {
    if(this.head.Pharmacy != null) {
      if(pharmacy && !this.head.Pharmacy.find(x => x === pharmacy)) {
        this.request = {};
        this.request.Pharmacy = pharmacy;
        this.request.PharmacyHead = this.head;
        this.dataService.claimPharmacy(this.request)
            .subscribe((req) => {
              if(req) {
                this.openSnackBar("Claiming request sent!", "OK");
              }
              else {
                this.openSnackBar("Unable to send a request", "Try again");
              }
            },
            (err: any) => console.log(err),
            () => console.log('Claiming request sent!'));
      }
    }
    else {
      if(pharmacy) {
        this.request = {};
        this.request.Pharmacy = pharmacy;
        this.request.PharmacyHead = this.head;
        this.dataService.claimPharmacy(this.request)
            .subscribe((req) => {
              if(req) {
                this.openSnackBar("Claiming request sent!", "OK");
              }
              else {
                this.openSnackBar("Unable to send a request", "Try again");
              }
            },
            (err: any) => console.log(err),
            () => console.log('Claiming request sent!'));
      }
    }
  }

  deleteMedicine(medicine: IMedicine){
    this.head.PharmacyMedicines = this.head.PharmacyMedicines.filter(x => x !== medicine);
    this.filteredMedicines = this.head.PharmacyMedicines;
    this.editedMedicine = true;
  }
  
  saveDeletedMedicines() {
    this.dataService.updatePharmacyHead(this.head)
      .subscribe((hd: IPharmacyHead) => {
        if(hd) {
          this.openSnackBar("Success! Medicine deleted", "OK");
          this.editedMedicine = false;
        }
        else {
          this.openSnackBar("Unable to delete Medicine", "Try again");
        }
      },
      (err: any) => console.log(err),
      () => console.log('Update sent!'));
  }

  applyFilterMedicines(filterValue: string) {
    console.log("applyFilterMedicines works!")
    if(filterValue) {
      this.filteredMedicines = this.filteredMedicines.filter(x => x.name.toLocaleLowerCase().includes(filterValue.toLocaleLowerCase()));
    }
    else {
      this.filteredMedicines = this.head.PharmacyMedicines;
    }
  }

  addMedicine() {
    let dialogRef = this.dialog.open(AddMedicineDialogComponent, {
      width: 'auto'
    });
    dialogRef.afterClosed().subscribe((newMedicine: IMedicine) => {
      if(newMedicine){
        this.head.PharmacyMedicines.push(newMedicine);
        this.filteredMedicines = this.head.PharmacyMedicines;
        if(this.editedMedicine == false) {
          this.editedMedicine = true;
        }
        this.openSnackBar("Success! Medicine added, please save changes now", "OK");
      }
      else {
        this.openSnackBar("Failed! Please try again", "OK");
      }
    }, () => this.openSnackBar("Failed! Please try again", "OK"));
  }

  addMedicinesFromList() {
    let dialogRef = this.dialog.open(ListMedicinesDialogComponent, {
      width: 'auto',
      height: 'auto'
    });
    dialogRef.afterClosed().subscribe((listMedicines: IMedicine[]) => {
      if(listMedicines){
        listMedicines.forEach((medicine) => {
          this.head.PharmacyMedicines = this.head.PharmacyMedicines.filter(x => x != medicine);
          this.head.PharmacyMedicines.push(medicine);
          this.filteredMedicines = this.head.PharmacyMedicines;
        });
        if(this.editedMedicine == false) {
          this.editedMedicine = true;
        }
        this.openSnackBar("Success! Medicines added, please save changes now", "OK");
      }
      else {
        this.openSnackBar("Failed! Please try again", "OK");
      }
    }, () => this.openSnackBar("Failed! Please try again", "OK"));
  }

  switchEditMedicineMode() {
    this.medicinesEditMode = !this.medicinesEditMode;
    if(this.editedMedicine == false) {
      this.editedMedicine = true;
    }
  }
  
  applyFilterPharmacies(filterValue: string) {
    console.log("applyFilterPharmacies works!")
    if(filterValue) {
      this.dataService.searchPharmacies(filterValue)
          .subscribe((pharmacy: IPharmacy[]) => {
            this.filteredPharmacies = pharmacy;
            this.head.Pharmacy.forEach((pharma) => {
              this.filteredPharmacies = this.filteredPharmacies.filter(x => x == pharma);
            });
          },
          (err: any) => console.log(err),
          () => console.log('Pharmacy data retrieved')); 
    }
    else {
      this.filteredPharmacies = this.pharmacies;
    }
  }

  logout() {
    this.authService.logout();
  }

  openPharmacyDialog(pharmacy: IPharmacy): void {
    this.dialog.open(PharmacyDialogComponent, {
      width: '450px',
      data: pharmacy
    });
  }

  openEditPharmacyDialog(pharmacy: IPharmacy): void {
    console.log(pharmacy);
    let dialogRef = this.dialog.open(EditPharmacyDialogComponent, {
      width: '450px',
      data: pharmacy
    });
    dialogRef.afterClosed().subscribe((editedPharmacy: IPharmacy) => {
      if(editedPharmacy) {
        this.head.Pharmacy = this.head.Pharmacy.filter(x => x !== pharmacy);
        this.head.Pharmacy.push(editedPharmacy);
        this.dataService.updatePharmacyHead(this.head)
            .subscribe((hd: IPharmacyHead) => {
              if(hd) {
                this.openSnackBar("Success! Pharmacy edited", "OK").onAction().subscribe(() => {
                  window.location.reload();
                });
              }
              else {
                this.openSnackBar("Pharmacy edit failed", "Try again");
              }
            },
            (err: any) => console.log(err),
            () => console.log('PharmacyHead data updated'));
      };
    });
  }

  openMedicineDialog(medicine: IMedicine): void {
    this.dialog.open(MedicineDialogComponent, {
      width: '450px',
      data: medicine
    });
  }

  openSnackBar(message: string, action: string) : MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, action, {
      duration: 5000,
    });
  }
}
