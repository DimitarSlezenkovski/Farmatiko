import { Component, OnInit } from '@angular/core';
import { IHealthFacilities, IHealthcareWorkers } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { MatDialog } from '@angular/material/dialog';
import { FacilityDialogComponent } from '../dialogs/facility-dialog/facility-dialog.component';
import { WorkerDialogComponent } from '../dialogs/worker-dialog/worker-dialog.component';
import { latLng, LatLng, tileLayer, marker, icon } from 'leaflet';
import { HttpClient } from '@angular/common/http';

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
  public lat;
  public lng;
  clicked = false;
  showMap: boolean = false;
  options = {
    layers: [
      tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors' })
    ],
    zoom: 8,
    center: latLng(41.61807, 21.74348)
  }
  
  constructor(private dataService: DataService, private dialog: MatDialog, private http: HttpClient) {
    
  }

  ngOnInit(): void {
    this.dataService.getFacilities()
        .subscribe((facility: IHealthFacilities[]) => {
          this.facilities = this.filteredFacilities = facility;
        },
        (err: any) => console.log(err),
        () => {
          this.appendFacilityMarkers(this.facilities)
          console.log('Facility data retrieved!');
        });

    this.dataService.getWorkers()
        .subscribe((worker: IHealthcareWorkers[]) => {
          this.workers = this.filteredWorkers = worker;
        },
        (err: any) => console.log(err),
        () => console.log('Facility data retrieved!'));

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        if (position) {
          this.lat = position.coords.latitude;
          this.lng = position.coords.longitude;
          let layer = marker([ this.lat, this.lng ], {
            icon: icon({
              iconSize: [ 25, 41 ],
              iconAnchor: [ 13, 41 ],
              iconUrl: 'assets/home-icon.png'
            })
          }).bindPopup("Вашата локација");
          this.options.layers.push(layer);
        }
      });
    }
  }

  appendFacilityMarkers(facils: IHealthFacilities[]) {
    this.options.layers = [];
    this.options.layers.push(tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'}));
    if(this.lat && this.lng) {
      this.options.layers.push(marker([ this.lat, this.lng ], {
        icon: icon({
          iconSize: [ 25, 41 ],
          iconAnchor: [ 13, 41 ],
          iconUrl: 'assets/home-icon.png'
        })
      }).bindPopup("Вашата локација"));
    }
    facils.forEach((facil) => {
      this.http.get<any>('https://jankuloski.xyz:8080/https://nominatim.openstreetmap.org/search/?country=Macedonia&city='+facil.municipality+'&street='+facil.address+'&format=json').subscribe(obj => {
        console.log(obj);  
        if(obj.length) {
            let layer = marker([ obj[0]?.lat, obj[0]?.lon ], {
              icon: icon({
                iconSize: [ 25, 41 ],
                iconAnchor: [ 13, 41 ],
                iconUrl: 'assets/hospital-icon.png'
              })
            }).bindPopup(facil.name);
            this.options.layers.push(layer);
          }
        }, error => console.error(error));
    });
  }

  applyFilterFacilities(filterValue: string) {
    console.log("applyFilterFacilities works!")
    if(filterValue) {
      this.dataService.searchFacilities(filterValue)
          .subscribe((facility: IHealthFacilities[]) => {
            this.filteredFacilities = facility;
          },
          (err: any) => console.log(err),
          () => {
            this.appendFacilityMarkers(this.filteredFacilities);
            if(this.showMap) {
              this.showMap = false;
              setTimeout(() => this.showMap = true, 300);
            }
            console.log('Facility data retrieved!');
          });
    }
    else {
      this.filteredFacilities = this.facilities;
      this.appendFacilityMarkers(this.facilities);
      if(this.showMap) {
        this.showMap = false;
        setTimeout(() => this.showMap = true, 300);
      }
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
  
  toggleMap() {
    this.showMap = !this.showMap;
  }

  refreshMap() {
    this.showMap = false;
    setTimeout(() => this.showMap = true, 300);
  }

}
