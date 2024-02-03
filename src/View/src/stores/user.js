import { ref, computed } from 'vue';
import { defineStore } from 'pinia'

export const useUserStore = defineStore("user", () => {
    const defaultUser = {
        id: '',
        userName: '',
        isAdmin: false
    };

    const user = ref({ ...defaultUser });

    const getUser = () => {
        return user.value;
    }

    const resetUser = () => {
        user.value = { ...defaultUser };
    }

    const setUser = (data) => {
        user.value = {...user.value, ...data };
    }

    const isLoggedIn = computed(() => {
        return !!user.value.id;
    });

    return {
        user,
        getUser,
        resetUser,
        setUser,
        isLoggedIn
    };
});