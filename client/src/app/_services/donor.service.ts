import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Donor } from '../_modals/donor';
import { DonorParams } from '../_modals/donorParams';
import { BasePageService } from './base-page.service';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class DonorService extends BasePageService<Donor, DonorParams> {
  baseUrl = environment.apiUrl + 'users';

  constructor(http: HttpClient) {
    super(http);
    this.params = new DonorParams();
  }

  resetParams() {
    this.params = new DonorParams();
  }

  addHttpParams(httpParams: HttpParams): HttpParams {
    httpParams = httpParams.append('minAge', this.params.minAge);
    httpParams = httpParams.append('maxAge', this.params.maxAge);
    httpParams = httpParams.append(
      'bloodGroup',
      encodeURIComponent(this.params.bloodGroup)
    );
    httpParams = httpParams.append('address', this.params.address);
    return httpParams;
  }
}
