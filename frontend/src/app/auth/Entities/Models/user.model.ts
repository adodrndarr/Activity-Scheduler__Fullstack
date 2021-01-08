export class UserToRegisterDTO {
    constructor(
        public userName: string,
        public lastName: string,
        public email: string,
        public password: string,
        public confirmPassword: string
    ) { }
}

export class UserToLoginDTO {
    constructor(
        public email: string,
        public password: string
    ) { }
}

export class CurrentUser {
    constructor(
        public id: string,
        public email: string,
        public isAdmin: boolean
    ) { }
}