export function isStringEmptyOrNull(str: string | null | undefined): boolean {
    return !str || str.length === 0;
}
