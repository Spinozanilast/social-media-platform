export interface ApiStore {
    get: (name: string) => any;
    set: (name: string, value: any) => void;
}