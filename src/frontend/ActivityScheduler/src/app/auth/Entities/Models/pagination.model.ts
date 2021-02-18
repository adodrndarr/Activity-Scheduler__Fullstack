export class Pagination {
    constructor(
        public totalCount?: number,
        public currentPage?: number,
        public pageSize?: number,
        public totalPages?: number,
        public hasPrevious?: boolean,
        public hasNext?: boolean
    ) { }
}
