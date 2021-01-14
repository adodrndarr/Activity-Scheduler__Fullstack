export interface RegisterResponseDTO {
    isRegistrationSuccessful: boolean;
    info: string;
    errorMessages: string[];
}

export interface LoginResponseDTO {
    userId: string;
    isLoginSuccessful: boolean;
    isAdmin: boolean;
    email: string;
    token: string;
    tokenExpirationDate: Date;
    errorMessage: string;
}
