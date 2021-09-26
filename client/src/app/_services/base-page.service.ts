import { HttpClient, HttpParams } from '@angular/common/http';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { BaseModal } from '../_modals/modal';
import { PageParams } from '../_modals/pageParams';
import { PageResult } from '../_modals/pagination';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

export abstract class BasePageService<
  Modal extends BaseModal,
  Param extends PageParams
> {
  cache: boolean;
  modal: Modal[] = [];
  modalCache = new Map();
  params: Param;
  modalMapCache = new Map();
  abstract baseUrl: string;

  constructor(public http: HttpClient, cache: boolean = true) {
    this.cache = cache;
  }

  getParams() {
    return this.params;
  }

  setParams(params: Param) {
    this.params = params;
  }

  abstract resetParams();

  getModals() {
    var response = this.modalMapCache.get(Object.values(this.params).join('-'));
    if (this.cache && response) {
      return this.getCacheData(response);
    }

    let params = getPaginationHeader(
      this.params.pageNumber,
      this.params.pageSize
    );

    params = this.addHttpParams(params);

    return getPaginatedResult<Modal>(this.baseUrl, params, this.http).pipe(
      map((response) => {
        this.updateCacheData(response);
        return response;
      })
    );
  }

  getModal(url: string, id) {
    var modal = this.modalCache.get(id);
    if (modal) {
      return of(this.modalCache.get(id));
    }

    return this.http.get<Modal>(url + '/' + id).pipe(
      map((response) => {
        this.cacheModal(response);
        return response;
      })
    );
  }

  updateCacheData(response: PageResult<Modal>) {
    if (this.cache) {
      this.modalMapCache.set(Object.values(this.params).join('-'), response);
      response.result.forEach((element) => {
        this.modalCache.set(element.id, element);
      });
    }
  }

  cacheModal(modal: Modal) {
    if (this.cache) {
      this.modalCache.set(modal.id, modal);
    }
  }

  getCacheData(response: PageResult<Modal>) {
    let items = response.result;
    for (let i = 0; i < items.length; i++) {
      items[i] = this.modalCache.get(items[i].id);
    }
    return of(response);
  }

  abstract addHttpParams(httpParams: HttpParams): HttpParams;
}
