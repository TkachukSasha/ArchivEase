import { ref } from 'vue'
import { getJwtFromCookie } from "@/api/utils.js";

export function useFetchPost(url, options) {
    const response = ref();

    const jwt = getJwtFromCookie();

    const request = async (data) => {
        const config = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: data ? JSON.stringify(data) : null,
            ...options,
        }

        if (jwt) {
            config.headers.Authorization = `Bearer ${jwt}`;
        }

        const res = await fetch(url, config)
        response.value = await res.json()
    }

    return { response, request }
}