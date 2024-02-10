<template>
  <div class="sign-in__container">
    <form class="sign-in__form" @submit.prevent="handleSubmit">
      <h2 class="sign-in__title">{{ $t('sign-in__form.title') }}</h2>

      <div class="sign-in__field" :class="{invalid: !form.userName.valid && form.userName.touched}">
        <label for="user_name" class="sign-in__label">{{ $t('sign-in__form.labels.user_name') }}</label>
        <input type="text" id="user_name" v-model="form.userName.value" @blur="form.userName.blur"/>
      </div>

      <div class="sign-in__field" :class="{invalid: !form.password.valid && form.password.touched}">
        <label for="password" class="sign-in__label">{{ $t('sign-in__form.labels.password') }}</label>
        <input type="password" id="password" v-model="form.password.value" @blur="form.password.blur"/>
      </div>

      <div class="sign-in__redirect">
        <span class="sign-in__question">{{ $t('sign-in__form.redirect.question') }}</span>
        <RouterLink to="/sign-up" class="sign__in-redirect_btn">{{ $t('sign-in__form.redirect.link') }}</RouterLink>
      </div>

      <button type="submit" class="sign-in__submit">{{ $t('sign-in__form.button') }}</button>
    </form>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { useForm } from '@/compositions/form.js'
import { useSignIn } from './compositions/signIn.js';
import Cookies from 'js-cookie';
import { eventBus } from "@/eventBus.js";

const router = useRouter();

const required = val => !!val;

const form = useForm(
    {
      userName: {
        value: '',
        validators: {required}
      },
      password: {
        value: '',
        validators: {required}
      }
    }
)

const handleSubmit  = async () =>{
  if (form.valid) {
    const userData = {
      userName: form.userName.value,
      password: form.password.value,
    };

    const { signInResponse } = await useSignIn(userData);

    const token = signInResponse.value.value?.token ?? '';

    Cookies.set('jwt', token);

    const authState = {
      isLoggedIn: true,
      isAdmin: signInResponse.value.value?.isAdmin
    }

    window.localStorage.setItem('authState', JSON.stringify(authState));

    eventBus.emit('authStateChanged', authState);

    await router.push('/');
  }
}
</script>

<style scoped>
@import "SignIn.css";
</style>