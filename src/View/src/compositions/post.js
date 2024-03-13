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

export function useUploadFiles(url, options) {
    const response = ref();
    const jwt = getJwtFromCookie();

    const request = async (files, additionalData) => {
        const formData = new FormData();

        for (let i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        if (additionalData) {
            Object.entries(additionalData).forEach(([key, value]) => {
                formData.append(key, value);
            });
        }

        const config = {
            method: 'POST',
            body: formData,
            ...options,
            headers: {
                ...(options && options.headers ? options.headers : {}),
                Authorization: jwt ? `Bearer ${jwt}` : undefined,
            },
        };

        const res = await fetch(url, config);
        response.value = res.blob();
    }

    return { response, request };
}