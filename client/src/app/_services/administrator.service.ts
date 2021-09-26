import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AdminRole, AdminRoleParams } from '../_modals/admin';
import { BasePageService } from './base-page.service';

@Injectable({
  providedIn: 'root',
})
export class AdministratorService extends BasePageService<
  AdminRole,
  AdminRoleParams
> {
  baseUrl = environment.apiUrl + 'admin/roles';

  constructor(http: HttpClient) {
    super(http, false);
    this.params = new AdminRoleParams();
  }

  resetParams() {
    this.params = new AdminRoleParams();
  }

  addHttpParams(httpParams: HttpParams): HttpParams {
    httpParams = httpParams.append('id', this.params.userId);
    httpParams = httpParams.append('name', this.params.name);
    httpParams = httpParams.append('userName', this.params.userName);
    httpParams = httpParams.append('role', this.params.role);
    return httpParams;
  }

  addRole(adminRole: AdminRole) {
    return this.http.post<AdminRole>(this.baseUrl, adminRole);
  }

  removeRole(adminRole) {
    return this.http.delete<AdminRole>(this.baseUrl, {
      body: adminRole,
      responseType: 'json',
    });
  }
}
