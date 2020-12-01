import { Component, OnInit } from '@angular/core';
import { IHealthFacilities, IHealthcareWorkers } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { MatDialog } from '@angular/material/dialog';
import { FacilityDialogComponent } from '../dialogs/facility-dialog/facility-dialog.component';
import { WorkerDialogComponent } from '../dialogs/worker-dialog/worker-dialog.component';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.css']
})
export class CounterComponent implements OnInit {
  public facilities: IHealthFacilities[] = [];
  public workers: IHealthcareWorkers[] = [];
  public filteredFacilities: IHealthFacilities[] = [];
  public filteredWorkers: IHealthcareWorkers[] = [];

  constructor(private dataService: DataService, private dialog: MatDialog) {
    
  }

  ngOnInit(): void {
    this.dataService.getFacilities()
        .subscribe((facility: IHealthFacilities[]) => {
          this.facilities = this.filteredFacilities = facility;
        },
        (err: any) => console.log(err),
        () => console.log('Facility data retrieved!'));

    this.dataService.getWorkers()
        .subscribe((worker: IHealthcareWorkers[]) => {
          this.workers = this.filteredWorkers = worker;
        },
        (err: any) => console.log(err),
        () => console.log('Facility data retrieved!'));
  }

  applyFilterFacilities(filterValue: string) {
    console.log("applyFilterFacilities works!")
    if(filterValue) {
      this.dataService.searchFacilities(filterValue)
          .subscribe((facility: IHealthFacilities[]) => {
            this.filteredFacilities = facility;
          },
          (err: any) => console.log(err),
          () => console.log('Facility data retrieved!'));
    }
    else {
      this.filteredFacilities = this.facilities;
    }
  }

  applyFilterWorkers(filterValue: string) {
    console.log("applyFilterWorkers works!")
    if(filterValue) {
      this.dataService.searchWorkers(filterValue)
          .subscribe((worker: IHealthcareWorkers[]) => {
            this.filteredWorkers = worker;
          },
          (err: any) => console.log(err),
          () => console.log('Worker data retrieved!'));
    }
    else {
      this.filteredWorkers = this.workers;
    }
  }

  openFacilityDialog(facility: IHealthFacilities): void {
    this.dialog.open(FacilityDialogComponent, {
      width: '450px',
      data: facility
    });
  }
  
  openWorkerDialog(worker: IHealthcareWorkers): void {
    this.dialog.open(WorkerDialogComponent, {
      width: '450px',
      data: worker
    });
  }
}
