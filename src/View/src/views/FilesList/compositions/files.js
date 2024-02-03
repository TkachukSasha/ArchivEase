import { ref } from 'vue'
import { useFetch } from '@/compositions/fetch.js'
import { fileEndpoints } from "@/api/apiEndpoints.js";

export async function useFiles({ page, results }) {
    const loaded = ref(false);

    const { response: files, request } = useFetch(
        fileEndpoints.default + `?page=${page}&results=${results}`
    );

    let totalItems = 0;

    if (!loaded.value) {
        await request();
        loaded.value = true;
    }

    return { files };
}