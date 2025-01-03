export default interface UrlCompleteItemData {
    id: number;
    name: string;
    url: string;
}

export const urlCompleteItems: UrlCompleteItemData[] = [
    {
        id: 1,
        name: 'Login',
        url: '/login',
    },
    {
        id: 2,
        name: 'Register',
        url: '/register',
    },
    {
        id: 3,
        name: 'Home',
        url: '/',
    },
];
