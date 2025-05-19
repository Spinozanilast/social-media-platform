import storage from '@/app/lib/api/storage';

const USER_ID_KEY = 'userId';

const UserStorage = {
    saveUserId: (userId: string) => storage.set(USER_ID_KEY, userId),
    getUserId: (userId: string) => storage.get(USER_ID_KEY),
};

export default UserStorage;
