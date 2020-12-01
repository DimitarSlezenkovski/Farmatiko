import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './shared/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { CoreModule } from './shared/core.module';

import { AuthGuard } from './shared/guards/auth.guard';
import { DataService } from './shared/data.service';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { KoronaComponent } from './korona/korona.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { MedicineDialogComponent } from './dialogs/medicine-dialog/medicine-dialog.component';
import { PharmacyDialogComponent } from './dialogs/pharmacy-dialog/pharmacy-dialog.component';
import { FacilityDialogComponent } from './dialogs/facility-dialog/facility-dialog.component';
import { WorkerDialogComponent } from './dialogs/worker-dialog/worker-dialog.component';
import { EditPharmacyDialogComponent } from './dialogs/edit-pharmacy-dialog/edit-pharmacy-dialog.component';
import { EditPharmacyHeadDialogComponent } from './dialogs/edit-pharmacy-head-dialog/edit-pharmacy-head-dialog.component';
import { PharmacyHeadDialogComponent } from './dialogs/pharmacy-head-dialog/pharmacy-head-dialog.component';
import { AddMedicineDialogComponent } from './dialogs/add-medicine-dialog/add-medicine-dialog.component';
import { ListMedicinesDialogComponent } from './dialogs/list-medicines-dialog/list-medicines-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    KoronaComponent,
    AdminComponent,
    DashboardComponent,
    LoginComponent,
    MedicineDialogComponent,
    PharmacyDialogComponent,
    FacilityDialogComponent,
    WorkerDialogComponent,
    EditPharmacyDialogComponent,
    EditPharmacyHeadDialogComponent,
    PharmacyHeadDialogComponent,
    AddMedicineDialogComponent,
    ListMedicinesDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'mapa', component: CounterComponent },
      { path: 'koronavirus', component: KoronaComponent },
      { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
      { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent }
    ]),
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule,
    CoreModule
  ],
  providers: [
    DataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
