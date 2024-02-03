<template>
  <select v-model="selectedLanguage" @change="changeLanguage" class="custom-select">
    <option v-for="(value, key) in supportedLanguages" :key="key" :value="key">{{ value }}</option>
  </select>
</template>

<script setup>
import { useI18n } from 'vue-i18n';

const { locale } = useI18n();

const supportedLanguages = {
  en: '🇬🇧 English',
  ua: '🇺🇦 Українська',
};

let selectedLanguage = locale.value;

const changeLanguage = () => {
  locale.value = selectedLanguage;
};
</script>

<style scoped>
@import "LanguagesSelector.css";
</style>