import { Component, OnInit } from '@angular/core';
import { IMedicine, IPharmacy } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { MatDialog } from '@angular/material/dialog';
import { MedicineDialogComponent } from '../dialogs/medicine-dialog/medicine-dialog.component';
import { PharmacyDialogComponent } from '../dialogs/pharmacy-dialog/pharmacy-dialog.component';
import { latLng, LatLng, tileLayer, marker, icon } from 'leaflet';
import { HttpClient } from '@angular/common/http';

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
        () => {
          this.appendPharmacyMarkers(this.pharmacies);
          console.log('Pharmacy data retrieved');
        });
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

  appendPharmacyMarkers(pharmas: IPharmacy[]) {
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
    pharmas.forEach((pharmacy) => {
      this.http.get<any>('https://jankuloski.xyz:8080/https://nominatim.openstreetmap.org/search/?country=Macedonia&city='+pharmacy.location+'&street='+pharmacy.address+'&format=json').subscribe(obj => {
        console.log(obj);  
        if(obj.length) {
            let layer = marker([ obj[0]?.lat, obj[0]?.lon ], {
              icon: icon({
                iconSize: [ 25, 41 ],
                iconAnchor: [ 13, 41 ],
                iconUrl: 'assets/pharmacy-icon.png'
              })
            }).bindPopup("Аптека: "+pharmacy.name);
            this.options.layers.push(layer);
          }
        }, error => console.error(error));
    });
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
          () => {
            this.appendPharmacyMarkers(this.filteredPharmacies);
            if(this.showMap) {
              this.showMap = false;
              setTimeout(() => this.showMap = true, 300);
            }
            console.log('Pharmacy data retrieved')
        }); 
    }
    else {
      this.filteredPharmacies = this.pharmacies;
      this.appendPharmacyMarkers(this.pharmacies);
      if(this.showMap) {
        this.showMap = false;
        setTimeout(() => this.showMap = true, 300);
      }
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

  toggleMap() {
    this.showMap = !this.showMap;
  }

  refreshMap() {
    this.showMap = false;
    setTimeout(() => this.showMap = true, 300);
  }

}
