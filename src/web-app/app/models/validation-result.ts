export interface ValidationError {
    propertyName: string;
    errorMessage: string;
}

export interface ValidationResult {
    isValid: boolean;
    errors: ValidationError[];
}
