export const MATCH_FIRST_LETTER_CAPITAL = '^[A-Z]+.*$';
export const MATCH_VALID_EMAIL = '^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$';
export const MATCH_VALID_PASSWORD = '^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{6,}$'; // 6 chars long, include 1 letter, 1 sign, 1 number
export const MATCH_NUMBERS = '^[-+]?\\d+$';
