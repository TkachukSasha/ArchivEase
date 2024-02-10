import { ref } from 'vue'
import { useFetch } from '@/compositions/fetch.js'
import { userEndpoints } from "@/api/apiEndpoints.js";

export async function useUsers({ page, results }) {
    const loaded = ref(false);

    const { response: users, request } = useFetch(
        userEndpoints.default + `?page=${page}&results=${results}`
    );

    let totalItems = 0;

    if (!loaded.value) {
        await request();
        loaded.value = true;
    }

    return { users, loaded };
}