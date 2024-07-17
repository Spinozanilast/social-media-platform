import { User } from "../models/shared/user";
import { getCookies } from "./cookies/server";
import UserApi from "./userApi";
import ApiConfigManager from "./util/configManager";

export const getUserApi = (): UserApi => {
    //const cookies = getCookies();
    const api = new UserApi({
        ...ApiConfigManager.getApiConfig(),
    });
    return api;
};

// export const getUser = (params?: {
//     onUnauthenticated?: () => void;
// }): User | null | undefined => {
//     const cookies = getCookies();
//     const api = new UserApi({
//         ...ApiConfigManager.getApiConfig,
//         store: cookies,
//     });
//     const user = api.user.getValue();
//     if (!user) params?.onUnauthenticated?.();
//     return user;
// };
