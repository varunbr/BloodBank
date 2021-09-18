export interface Pagination {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
}

export class PageResult<T> {
  result: T;
  pagination: Pagination;
}
