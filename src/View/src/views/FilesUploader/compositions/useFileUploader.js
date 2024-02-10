import { ref } from "vue";
import { useUploadFiles } from "@/compositions/post.js";
import { fileEndpoints } from "@/api/apiEndpoints.js";

export async function useFileUploader(files, key){
    const loaded = ref(false);

    const url = key === "encode" ? fileEndpoints.encode : fileEndpoints.decode;

    const {response: uploadResponse, request} = useUploadFiles(url);

    if (!loaded.value) {
        await request(files);
        loaded.value = true;
    }

    return { uploadResponse };
}