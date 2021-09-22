import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { PageResult } from '../_modals/pagination';

export function getPaginationHeader(pageNumber: number, pageSize: number) {
  let params = new HttpParams();
  params = params.append('pageNumber', pageNumber);
  params = params.append('pageSize', pageSize);
  return params;
}

export function getPaginatedResult<T>(url, params, http: HttpClient) {
  let pagination = new PageResult<T>();
  return http.get<T[]>(url, { observe: 'response', params: params }).pipe(
    map((response) => {
      pagination.result = response.body;
      pagination.pagination = JSON.parse(response.headers.get('Pagination'));
      return pagination;
    })
  );
}
