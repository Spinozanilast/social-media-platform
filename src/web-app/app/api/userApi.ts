import { LoginRequest } from "../models/dto/userDto";
import axios from "axios";

export default class UserApi {
    private serverUrl: string;
    private userLoginEndpoint: string = "/user/login";
    private userRegisterEndpoint: string = "/user/register";

    constructor(serverUrl: string) {
        this.serverUrl = serverUrl;
    }

    async registerHandler(data: LoginRequest) {
        axios
            .post(this.serverUrl, data, {
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json",
                },
            })
            .then((response) => {
                console.log(response);
            });
    }
}
