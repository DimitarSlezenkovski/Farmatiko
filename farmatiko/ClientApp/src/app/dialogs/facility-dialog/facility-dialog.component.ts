import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IHealthFacilities } from '../../shared/interfaces';
import { latLng, LatLng, tileLayer, marker, icon } from 'leaflet';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-facility-dialog',
  templateUrl: './facility-dialog.component.html',
  styleUrls: ['./facility-dialog.component.css']
})
export class FacilityDialogComponent implements OnInit {
  facility: IHealthFacilities;
  mapShown: boolean = true;
  options = {
    layers: [
      tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors' })
    ],
    zoom: 7,
    center: latLng(41.61807, 21.74348)
  }

  constructor(private dialogRef: MatDialogRef<FacilityDialogComponent>, @Inject(MAT_DIALOG_DATA) data, private http: HttpClient) {
    this.facility = data;
    if(this.facility) {
      this.addMarkers();
    }
  }

  ngOnInit(): void {
  }

  addMarkers() {
    this.http.get<any>('https://jankuloski.xyz:8080/https://nominatim.openstreetmap.org/search/?country=Macedonia&city='+this.facility?.municipality+'&street='+this.facility?.address+'&format=json').subscribe(obj => {
      console.log(obj);  
      if(obj.length) {
          let layer = marker([ obj[0]?.lat, obj[0]?.lon ], {
            icon: icon({
              iconSize: [ 25, 41 ],
              iconAnchor: [ 13, 41 ],
              iconUrl: 'assets/hospital-icon.png'
            })
          }).bindPopup(this.facility?.name);
          this.options.layers.push(layer);
          this.options.center = latLng(obj[0]?.lat, obj[0]?.lon);
          this.options.zoom = 13;
        }
      }, error => console.error(error),
      () => {
        this.mapShown = false;
        setTimeout(() => this.mapShown = true, 300);
      });
  }

  close() {
    this.dialogRef.close();
  }

}
