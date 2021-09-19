import { HttpClient, HttpParams } from '@angular/common/http';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PageParams } from '../_modals/pageParams';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

export abstract class BasePageService<Modal, Param extends PageParams> {
  modal: Modal[] = [];
  params: Param;
  modalCache = new Map();
  abstract baseUrl: string;

  constructor(private http: HttpClient) {}

  getParams() {
    return this.params;
  }

  setParams(params: Param) {
    this.params = params;
  }

  abstract resetParams();

  getModals() {
    var response = this.modalCache.get(Object.values(this.params).join('-'));
    if (response) {
      return of(response);
    }

    let params = getPaginationHeader(
      this.params.pageNumber,
      this.params.pageSize
    );

    params = this.addHttpParams(params);

    return getPaginatedResult<Modal>(this.baseUrl, params, this.http).pipe(
      map((response) => {
        this.modalCache.set(Object.values(this.params).join('-'), response);
        return response;
      })
    );
  }

  abstract addHttpParams(httpParams: HttpParams): HttpParams;
}
