import { ref } from 'vue'
import {useDownload, useFetch} from '@/compositions/fetch.js'
import { fileEndpoints } from "@/api/apiEndpoints.js";

export async function useFiles({ page, results }) {
    const loaded = ref(false);

    const userId = window.localStorage.getItem('id');

    const { response: files, request } = useFetch(
        fileEndpoints.default + `?userId=${userId}&page=${page}&results=${results}`
    );

    if (!loaded.value) {
        await request();
        loaded.value = true;
    }

    return { files, loaded };
}

export async function useFileDownloader(fileName, key){
    const loaded = ref(false);

    const url = key === 'download' ? fileEndpoints.download + `/${fileName}` : fileEndpoints.decode + `/${fileName}`;

    const method = key === 'download' ? 'GET' : 'POST';

    const {response: downloadResponse, request} = useDownload(url,  {}, method);

    if (!loaded.value) {
        await request();
        loaded.value = true;
    }

    return { downloadResponse, loaded };
}
