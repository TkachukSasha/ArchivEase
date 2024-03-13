import { ref } from 'vue'
import {getJwtFromCookie} from "@/api/utils.js";

export function useFetch(url, options) {
    const response = ref();

    const jwt = getJwtFromCookie();

    const request = async () => {
        const config = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            ...options
        }

        if (jwt) {
            config.headers.Authorization = `Bearer ${jwt}`;
        }

        const res = await fetch(url, config)
        response.value = await res.json()
    }

    return {response, request}
}

export function useDownload(url, options, method){
    const response = ref();

    const jwt = getJwtFromCookie();

    const request = async () => {
        const config = {
            method: method,
            ...options,
            headers: {
                ...(options && options.headers ? options.headers : {}),
                Authorization: jwt ? `Bearer ${jwt}` : undefined,
            },
        };

        const res = await fetch(url, config)
        response.value = res.blob();
    }

    return {response, request}
}