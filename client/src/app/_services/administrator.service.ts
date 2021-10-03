import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Role, RoleParams } from '../_modals/admin';
import { BasePageService } from './base-page.service';

@Injectable({
  providedIn: 'root',
})
export class AdministratorService extends BasePageService<
  Role,
  RoleParams
> {
  baseUrl = environment.apiUrl + 'admin/roles';

  constructor(http: HttpClient) {
    super(http, false);
    this.params = new RoleParams();
  }

  resetParams() {
    this.params = new RoleParams();
  }

  addHttpParams(httpParams: HttpParams): HttpParams {
    httpParams = httpParams.append('id', this.params.userId);
    httpParams = httpParams.append('name', this.params.name);
    httpParams = httpParams.append('userName', this.params.userName);
    httpParams = httpParams.append('role', this.params.role);
    return httpParams;
  }

  addRole(adminRole: Role) {
    return this.http.post<Role>(this.baseUrl, adminRole);
  }

  removeRole(adminRole) {
    return this.http.delete<Role>(this.baseUrl, {
      body: adminRole,
      responseType: 'json',
    });
  }
}
