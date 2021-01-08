export class ActivityEntity {
    constructor(
        public id: string,
        public name: string,
        public imageUrl: string,
        public itemQuantity: number,
        public minUserCount: number,
        public maxUserCount: number,
        public description: string,
        public location: string
    ) { }
}
