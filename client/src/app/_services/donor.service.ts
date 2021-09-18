import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Donor } from '../_modals/donor';
import { DonorParams } from '../_modals/donorParams';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class DonorService {
  donors: Donor[] = [];
  donorParams: DonorParams;
  donorCache = new Map();

  constructor(private http: HttpClient) {
    this.donorParams = new DonorParams();
  }

  getDonorParams() {
    return this.donorParams;
  }

  setDonorParams(params: DonorParams) {
    this.donorParams = params;
  }

  resetDonorParams() {
    this.donorParams = new DonorParams();
    return this.donorParams;
  }

  getDonors() {
    var response = this.donorCache.get(
      Object.values(this.donorParams).join('-')
    );
    if (response) {
      return of(response);
    }

    let params = getPaginationHeader(
      this.donorParams.pageNumber,
      this.donorParams.pageSize
    );

    params = params.append('minAge', this.donorParams.minAge);
    params = params.append('maxAge', this.donorParams.maxAge);
    params = params.append('bloodGroup', encodeURIComponent(this.donorParams.bloodGroup));
    params = params.append('address', this.donorParams.address);

    return getPaginatedResult<Donor>(
      environment.apiUrl + 'users',
      params,
      this.http
    ).pipe(
      map((response) => {
        this.donorCache.set(Object.values(this.donorParams).join('-'), response);
        return response;
      })
    );
  }
}
