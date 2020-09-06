export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginationResult<T> { // generic class used to paginate any type such as User, Messages etc
    result: T;
    pagination: Pagination;
}