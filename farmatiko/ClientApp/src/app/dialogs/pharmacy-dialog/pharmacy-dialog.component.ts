import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IPharmacy } from '../../shared/interfaces';
import { latLng, LatLng, tileLayer, marker, icon } from 'leaflet';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-pharmacy-dialog',
  templateUrl: './pharmacy-dialog.component.html',
  styleUrls: ['./pharmacy-dialog.component.css']
})
export class PharmacyDialogComponent implements OnInit {
  pharmacy: IPharmacy;
  mapShown: boolean = true;
  options = {
    layers: [
      tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors' })
    ],
    zoom: 7,
    center: latLng(41.61807, 21.74348)
  }

  constructor(private dialogRef: MatDialogRef<PharmacyDialogComponent>, @Inject(MAT_DIALOG_DATA) data, private http: HttpClient) {
    this.pharmacy = data;
    if(this.pharmacy) {
      this.addMarkers();
    }
  }

  ngOnInit(): void {
  }

  addMarkers() {
    this.http.get<any>('https://jankuloski.xyz:8080/https://nominatim.openstreetmap.org/search/?country=Macedonia&city='+this.pharmacy?.location+'&street='+this.pharmacy?.address+'&format=json').subscribe(obj => {
      console.log(obj);  
      if(obj.length) {
          let layer = marker([ obj[0]?.lat, obj[0]?.lon ], {
            icon: icon({
              iconSize: [ 25, 41 ],
              iconAnchor: [ 13, 41 ],
              iconUrl: 'assets/pharmacy-icon.png'
            })
          }).bindPopup("Аптека: "+this.pharmacy?.name);
          this.options.layers.push(layer);
          this.options.center = latLng(obj[0]?.lat, obj[0]?.lon);
          this.options.zoom = 13;
        }
      }, error => console.error(error),
      () => {
        this.mapShown = false;
        setTimeout(() => this.mapShown = true, 200);
      });
  }

  close() {
    this.dialogRef.close();
  }


}