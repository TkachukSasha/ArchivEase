import { ref } from 'vue'
import { useFetchPost } from '@/compositions/post.js'
import { userEndpoints } from "@/api/apiEndpoints.js";

export async function useSignUp(data){
    const loaded = ref(false);

    const {response: signUpResponse, request} = useFetchPost(userEndpoints.sign_up);

    if (!loaded.value) {
        await request(data);
        loaded.value = true;
    }

    return { signUpResponse };
}