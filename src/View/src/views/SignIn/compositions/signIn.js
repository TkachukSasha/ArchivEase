import { ref } from 'vue'
import { useFetchPost } from '@/compositions/post.js'
import { userEndpoints } from "@/api/apiEndpoints.js";

export async function useSignIn(data){
    const loaded = ref(false);

    const {response: signInResponse, request} = useFetchPost(userEndpoints.sign_in);

    if (!loaded.value) {
        await request(data);
        loaded.value = true;
    }

    return { signInResponse };
}