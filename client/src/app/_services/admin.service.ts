import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Bank } from '../_modals/bank';
import { BankParams } from '../_modals/bankParams';
import { BasePageService } from './base-page.service';

@Injectable({
  providedIn: 'root',
})
export class AdminService extends BasePageService<Bank, BankParams> {
  baseUrl = environment.apiUrl + 'admin';

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
    return httpParams;
  }

  getBank(id: number) {
    return this.getModal(this.baseUrl, id);
  }

  updateBank(body) {
    return this.http.put<Bank>(this.baseUrl, body).pipe(
      map((response) => {
        this.cacheModal(response);
        return response;
      })
    );
  }

  updateBankRoles(body) {
    return this.http.put<Bank>(this.baseUrl + '/bank-role', body).pipe(
      map((response) => {
        this.cacheModal(response);
        return response;
      })
    );
  }

  registerBank(body) {
    return this.http.post(this.baseUrl + '/register-bank', body);
  }
}
