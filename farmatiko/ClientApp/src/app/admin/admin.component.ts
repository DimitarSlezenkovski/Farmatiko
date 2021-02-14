import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { IPharmacyHead, IPharmacyHeadRequest, IPharmacy } from '../shared/interfaces';
import { DataService } from '../shared/data.service';
import { EditPharmacyHeadDialogComponent } from '../dialogs/edit-pharmacy-head-dialog/edit-pharmacy-head-dialog.component';
import { PharmacyDialogComponent } from '../dialogs/pharmacy-dialog/pharmacy-dialog.component';
import { PharmacyHeadDialogComponent } from '../dialogs/pharmacy-head-dialog/pharmacy-head-dialog.component';
import { AuthService } from '../shared/services/auth.service';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  public heads: IPharmacyHead[] = [];
  public requests: IPharmacyHeadRequest[] = [];
  public head: IPharmacyHead = {
    Email: '',
    Passwd: '',
    Name: ''
  };
  public adminHead: IPharmacyHead;

  constructor(private dataService: DataService, private authService: AuthService, private dialog: MatDialog, private snackBar: MatSnackBar, private router: Router) {

  }

  ngOnInit(): void {
    this.authService.getUser()
        .subscribe((data) => {
          console.log(data);
          this.adminHead = data;
        },
        (err: any) => console.log(err),
        () => console.log('User data retrieved'));

    this.dataService.getPharmacyHeads()
        .subscribe((pHeads: IPharmacyHead[]) => {
          this.heads = pHeads;
        },
        (err: any) => console.log(err),
        () => console.log("PharmacyHead data retrieved"));

    this.dataService.getClaimingRequests()
        .subscribe((pRequests: IPharmacyHeadRequest[]) => {
          this.requests = pRequests;
        },
        (err: any) => console.log(err),
        () => console.log("PharmacyHead data retrieved"));
  }

  createHead() {
    this.dataService.insertPharmacyHead(this.head)
        .subscribe((cHead) => {
            this.heads.push(cHead);
            this.openSnackBar("New head created!","OK");
        },
        (err: any) => console.log(err),
        () => console.log("PharmacyHead inserted"));
    this.head = {
      Email: '',
      Passwd: '',
      Name: ''
    };
  }

  deletePharmacyHead(dHead: IPharmacyHead) {
    this.dataService.deletePharmacyHead(dHead.id)
        .subscribe((status: boolean) => {
          if(status) {
            this.openSnackBar("Head deleted!","OK");
          }
        },
        (err: any) => console.log(err),
        () => {
          console.log("PharmacyHead deleted");
          location.reload();
        });
  }

  openEditPharmacyHeadDialog(eHead: IPharmacyHead) {
    let dialogRef = this.dialog.open(EditPharmacyHeadDialogComponent, {
      width: '450px',
      data: eHead
    });
    dialogRef.afterClosed().subscribe((editedHead: IPharmacyHead) => {
      if(editedHead) {
          console.log(editedHead);
          this.heads = this.heads.filter(x => x !== eHead);
          this.heads.push(editedHead);
          this.dataService.updatePharmacyHead(editedHead)
              .subscribe((hd: IPharmacyHead) => {
                  this.openSnackBar("Success! PharmacyHead edited", "OK").onAction().subscribe(() => {
                    location.reload();
                  });
              },
              (err: any) => console.log(err),
              () => console.log('PharmacyHead data updated'));
      }
    });
  }

  openPharmacyDialog(pharmacy: IPharmacy): void {
    this.dialog.open(PharmacyDialogComponent, {
      width: '450px',
      data: pharmacy
    });
  }

  openPharmacyHeadDialog(hd: IPharmacyHead): void {
    this.dialog.open(PharmacyHeadDialogComponent, {
      width: '450px',
      data: hd
    });
  }

  rejectRequest(req: IPharmacyHeadRequest) {
    this.dataService.deleteClaimingRequest(req)
        .subscribe((status: boolean) => {
          if(status) {
            this.openSnackBar("Request processed!","OK");
          }
        },
        (err: any) => console.log(err),
        () => {
          console.log("PharmacyHeadRequest deleted");
          location.reload();
        });
  }

  approveRequest(req: IPharmacyHeadRequest) {
    if(req) {
      if (req.PharmacyHead.Pharmacy == null){
        req.PharmacyHead.Pharmacy = [];
      }
    req.PharmacyHead.Pharmacy.push(req.Pharmacy);
    this.dataService.updatePharmacyHead(req.PharmacyHead)
        .subscribe(() => {
          this.rejectRequest(req);
        },
        (err: any) => console.log(err),
        () => {
          console.log("PharmacyHead updated");
          location.reload();
        })
    }
  }

  logout() {
    this.authService.logout();
  }

  openSnackBar(message: string, action: string) : MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, action, {
      duration: 5000,
    });
  }

}
