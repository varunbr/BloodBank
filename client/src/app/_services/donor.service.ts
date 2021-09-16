import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Donor } from '../_modals/donor';

@Injectable({
  providedIn: 'root',
})
export class DonorService {
  donors: Donor[] = [];
  constructor(private http: HttpClient) {}

  getDonors() {
    return this.http
      .get(environment.apiUrl + 'users')
      .pipe(map((response: Donor[]) => {
        this.donors = response
        return response;
      }));
  }
}
