export class DefaultDate {
    constructor(
        public date: Date,
        public isFreeToSchedule: boolean,
        public isScheduled?: boolean
    ) { }
}
