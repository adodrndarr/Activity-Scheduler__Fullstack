export class PaginationRequest {
    constructor(
        public page?: string,
        public size?: string,
        public searchTerm?: string
    ) { }
}
