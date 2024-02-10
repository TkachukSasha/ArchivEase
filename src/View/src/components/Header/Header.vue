<template>
  <div class="header__wrapper">
    <h1
        class="header__title"
        @click="goBack"
    >ArchivEase</h1>

    <ul
        ref="headerOptions"
        class="header__options"
        :style="{ maxHeight: headerOptionsHeight }"
    >
      <li class="header__option">
        <LanguageSelector />
      </li>
      <li
          class="header__option"
          v-if="!isLoggedIn"
      >
        <RouterLink
            to="/sign-in"
            class="header__option-text"
        > {{ $t('header__buttons.sign-in') }}</RouterLink>
      </li>
      <li
          class="header__option"
          v-if="!isLoggedIn"
      >
        <RouterLink
            to="/sign-up"
            class="header__option-text"
        > {{ $t('header__buttons.sign-up') }}</RouterLink>
      </li>
      <li
          class="header__option"
          v-if="isLoggedIn"
      >
        <RouterLink
            to="/files"
            class="header__option-text"
        >{{ $t('header__buttons.files') }}</RouterLink>
      </li>
      <li
          class="header__option"
          v-if="isLoggedIn && isAdmin"
      >
        <RouterLink
            to="/users"
            class="header__option-text"
        >{{ $t('header__buttons.users') }}</RouterLink>
      </li>
      <li
          class="header__option"
          v-if="isLoggedIn"
      >
        <RouterLink
            to="/"
            class="header__option-text"
            @click="logOut"
        >{{ $t('header__buttons.logout') }}</RouterLink>
      </li>
    </ul>

    <div class="header__menu" @click="toggleHeaderMenu">
      <font-awesome-icon icon="fa-solid fa-bars" />
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import LanguageSelector from "@/components/LanguagesSelector/LanguagesSelector.vue";
import { useRouter } from 'vue-router';
import Cookies from 'js-cookie';
import {eventBus} from "@/eventBus.js";

const router = useRouter();

const isLoggedIn = ref(!!JSON.parse(window.localStorage.getItem('authState'))?.isLoggedIn);
const isAdmin = ref(!!JSON.parse(window.localStorage.getItem('authState'))?.isAdmin);
const headerOptionsHeight = ref('0px');
const isToggle = ref(false);

function goBack(){
  router.push({ name: 'main' });
}

function toggleHeaderMenu() {
  headerOptionsHeight.value = isToggle.value ? '0px' : '300px';
  isToggle.value = !isToggle.value;
}

async function logOut() {
  window.localStorage.removeItem('isLoggedIn');

  isLoggedIn.value = false;

  Cookies.remove('jwt');
}

eventBus.on('authStateChanged', (newAuthState) => {
  isLoggedIn.value = newAuthState.isLoggedIn;
  isAdmin.value = newAuthState.isAdmin;
});
</script>

<style scoped>
@import "Header.css";
</style>