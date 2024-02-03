<template>
  <div class="sign-up__container">
    <form class="sign-up__form" @submit.prevent="handleSubmit ">
      <h2 class="sign-up__title">{{ $t('sign-up__form.title') }}</h2>

      <div class="sign-up__field" :class="{invalid: !form.userName.valid && form.userName.touched}">
        <label for="user_name" class="sign-up__label">{{ $t('sign-up__form.labels.user_name') }}</label>
        <input type="text" id="user_name" v-model="form.userName.value" @blur="form.userName.blur"/>
      </div>

      <div class="sign-up__field" :class="{invalid: !form.password.valid && form.password.touched}">
        <label for="password" class="sign-up__label">{{ $t('sign-up__form.labels.password') }}</label>
        <input type="password" id="password" v-model="form.password.value" @blur="form.password.blur"/>
      </div>

      <div class="sign-up__redirect">
        <span class="sign-up__question">{{ $t('sign-up__form.redirect.question') }}</span>
        <RouterLink to="/sign-in" class="sign__up-redirect_btn">{{ $t('sign-up__form.redirect.link') }}</RouterLink>
      </div>

      <button type="submit" class="sign-up__submit">{{ $t('sign-up__form.button') }}</button>
    </form>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { useForm } from '@/compositions/form.js'
import { useSignUp } from './compositions/signUp.js';
import { useUserStore } from '@/stores/user.js';
import Cookies from 'js-cookie';

const router = useRouter();

const userStore = useUserStore();

const required = val => !!val;
const minLength = num => val => val.length >= num;

const form = useForm(
    {
      userName: {
        value: '',
        validators: {required}
      },
      password: {
        value: '',
        validators: {required, minLength: minLength(8)}
      }
    }
)

const handleSubmit  = async () =>{
  if (form.valid) {
    const userData = {
      userName: form.userName.value,
      password: form.password.value,
    };

    const { signUpResponse } = await useSignUp(userData);

    const user = {
      id: signUpResponse.value.value?.id,
      userName: signUpResponse.value.value?.userName,
      isAdmin: signUpResponse.value.value?.isAdmin
    };

    userStore.setUser(user);

    const token = signUpResponse.value.value?.token ?? '';

    Cookies.set('jwt', token);

    await router.push('/');
  }
}
</script>

<style scoped>
@import "SignUp.css";
</style>