import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Bank } from '../_modals/bank';
import { BankParams } from '../_modals/bankParams';
import { BasePageService } from './base-page.service';

@Injectable({
  providedIn: 'root',
})
export class BankService extends BasePageService<Bank, BankParams> {
  baseUrl = environment.apiUrl + 'banks';

  constructor(http: HttpClient) {
    super(http);
    this.params = new BankParams();
  }

  resetParams() {
    this.params = new BankParams();
  }

  addHttpParams(httpParams: HttpParams): HttpParams {
    httpParams = httpParams.append('address', this.params.address);
    httpParams = httpParams.append('name', this.params.bankName);
    httpParams = httpParams.append(
      'bloodGroup',
      encodeURIComponent(this.params.bloodGroup)
    );
    return httpParams;
  }
}
