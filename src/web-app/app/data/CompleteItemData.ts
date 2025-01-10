export default interface CompleteItemData {
    id: number;
    name: string;
    url: string;
}

export const AccountUrlsCompleteItems: CompleteItemData[] = [
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
