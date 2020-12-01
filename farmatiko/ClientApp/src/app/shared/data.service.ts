import { Injectable,  } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { IHealthFacilities, IHealthcareWorkers, IMedicine, IPandemic, IPharmacy, IPharmacyHead, IPharmacyHeadRequest } from './interfaces';
import { environment } from '../../environments/environment';

@Injectable()
export class DataService {
    baseFacilitiesUrl: string = `${environment.baseApiUrl}api/facilities`;
    baseWorkersUrl: string = `${environment.baseApiUrl}api/workers`;
    baseMedicineUrl: string = `${environment.baseApiUrl}api/medicines`;
    basePandemicUrl: string = `${environment.baseApiUrl}api/pandemic`;
    basePharmacyUrl: string = `${environment.baseApiUrl}api/pharmacy`;
    basePharmacyHead: string = `${environment.baseApiUrl}api/pharmacyhead`;

    constructor(private http: HttpClient) {

    }

    //Facility GET
    getFacilities() : Observable<IHealthFacilities[]> {
        return this.http.get<IHealthFacilities[]>(this.baseFacilitiesUrl)
                   .pipe(
                        map((facilities: IHealthFacilities[]) => {
                            return facilities;
                        }),
                        catchError(this.handleError)
                   );
    }
    searchFacilities(searchquery: string) : Observable<IHealthFacilities[]> {
        return this.http.get<IHealthFacilities[]>(this.baseFacilitiesUrl + '/search/' + searchquery)
                   .pipe(
                        map((facilities: IHealthFacilities[]) => {
                            return facilities;
                        }),
                        catchError(this.handleError)
                   );
    }
    getFacility(id: string) : Observable<IHealthFacilities> {
        return this.http.get<IHealthFacilities>(this.baseFacilitiesUrl + '/' + id)
                   .pipe(catchError(this.handleError));
    }


    //Worker GET
    getWorkers() : Observable<IHealthcareWorkers[]> {
        return this.http.get<IHealthcareWorkers[]>(this.baseWorkersUrl)
                   .pipe(
                        map((workers: IHealthcareWorkers[]) => {
                            return workers;
                        }),
                        catchError(this.handleError)
                   );
    }
    searchWorkers(searchquery: string) : Observable<IHealthcareWorkers[]> {
        return this.http.get<IHealthcareWorkers[]>(this.baseWorkersUrl + '/search/' + searchquery)
                   .pipe(
                        map((workers: IHealthcareWorkers[]) => {
                            return workers;
                        }),
                        catchError(this.handleError)
                   );
    }
    getWorker(id: string) : Observable<IHealthcareWorkers> {
        return this.http.get<IHealthcareWorkers>(this.baseWorkersUrl + '/' + id)
                   .pipe(catchError(this.handleError));
    }


    //Medicine GET
    getMedicines() : Observable<IMedicine[]> {
        return this.http.get<IMedicine[]>(this.baseMedicineUrl)
                   .pipe(
                        map((medicines: IMedicine[]) => {
                            return medicines;
                        }),
                        catchError(this.handleError)
                   );
    }
    searchMedicines(searchquery: string) : Observable<IMedicine[]> {
        return this.http.get<IMedicine[]>(this.baseMedicineUrl + '/search/' + searchquery)
                   .pipe(
                        map((medicines: IMedicine[]) => {
                            return medicines;
                        }),
                        catchError(this.handleError)
                   );
    }
    getMedicine(id: string) : Observable<IMedicine> {
        return this.http.get<IMedicine>(this.baseMedicineUrl + '/' + id)
                   .pipe(catchError(this.handleError));
    }


    getPandemic() : Observable<IPandemic> {
        return this.http.get<IPandemic>(this.basePandemicUrl)
                   .pipe(catchError(this.handleError));
    }


    //Pharmacy GET
    getPharmacies() : Observable<IPharmacy[]> {
        return this.http.get<IPharmacy[]>(this.basePharmacyUrl)
                   .pipe(
                        map((pharmacies: IPharmacy[]) => {
                            return pharmacies;
                        }),
                        catchError(this.handleError)
                   );
    }
    searchPharmacies(searchquery: string) : Observable<IPharmacy[]> {
        return this.http.get<IPharmacy[]>(this.basePharmacyUrl + '/search/' + searchquery)
                   .pipe(
                        map((pharmacies: IPharmacy[]) => {
                            return pharmacies;
                        }),
                        catchError(this.handleError)
                   );
    }
    getPharmacy(id: string) : Observable<IPharmacy> {
        return this.http.get<IPharmacy>(this.basePharmacyUrl + '/' + id)
                   .pipe(catchError(this.handleError));
    }

    //PharmacyHead GET
    getPharmacyHeads() : Observable<IPharmacyHead[]> {
        return this.http.get<IPharmacyHead[]>(this.basePharmacyHead)
                   .pipe(
                        map((pharmacyheads: IPharmacyHead[]) => {
                            return pharmacyheads;
                        }),
                        catchError(this.handleError)
                   );
    }
    getClaimingRequests() : Observable<IPharmacyHeadRequest[]> {
        return this.http.get<IPharmacyHeadRequest[]>(this.basePharmacyHead + '/requests')
                   .pipe(
                        map((requests: IPharmacyHeadRequest[]) => {
                            return requests;
                        }),
                        catchError(this.handleError)
                   );
    }
    loginPharmacyHead(email: string, passwd: string) : Observable<any> {
        let postData = {username : email ,password :passwd};
        return this.http.post<any>(this.basePharmacyHead + '/login', postData)
                   .pipe(
                        map((data) => {
                            return data;
                        }),
                        catchError(this.handleError)
                    );
    }
    getPharmacyHead(id: string) : Observable<IPharmacyHead> {
        return this.http.get<IPharmacyHead>(this.basePharmacyHead + '/' + id)
                   .pipe(catchError(this.handleError));
    }
    //PharmacyHead POST
    insertPharmacyHead(head: IPharmacyHead) : Observable<IPharmacyHead> {
        return this.http.post<IPharmacyHead>(this.basePharmacyHead + '/add', head)
                   .pipe(
                        map((data) => {
                            return data;
                        }),
                        catchError(this.handleError)
                    );
    }
    claimPharmacy(req: IPharmacyHeadRequest) : Observable<IPharmacyHeadRequest> {
        return this.http.post<IPharmacyHeadRequest>(this.basePharmacyHead + '/requests', req)
                    .pipe(
                        map((data) => {
                            return data;
                        }),
                        catchError(this.handleError)
                    );
    }
    //PharmacyHead PUT
    updatePharmacyHead(head: IPharmacyHead) : Observable<IPharmacyHead> {
        return this.http.put<IPharmacyHead>(this.basePharmacyHead + '/update', head)
                   .pipe(
                        map((data) => {
                            return data;
                        }),
                        catchError(this.handleError)
                    );
    }
    //PharmacyHead DELETE
    deletePharmacyHead(id: string) : Observable<boolean> {
        return this.http.delete<boolean>(this.basePharmacyHead + '/delete/' + id)
                   .pipe(catchError(this.handleError));
    }
    deleteClaimingRequest(id: string) : Observable<boolean> {
        return this.http.delete<boolean>(this.basePharmacyHead + '/requests/' + id)
                   .pipe(catchError(this.handleError));
    }

    private handleError(error: HttpErrorResponse) {
        console.error('server error:', error);
        if (error.error instanceof Error) {
          let errMessage = error.error.message;
          return Observable.throw(errMessage);
        }
        return Observable.throw(error || 'ASP.NET Core server error');
    }

}
