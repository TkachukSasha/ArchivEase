<template>
  <div class="app__features-wrapper">
    <ul class="app__features">
      <li class="app__feature-option" v-for="(option, index) in localizedOptions" :key="index" @click="onSubmit(option.key)">
        <div class="app__feature-option-content">
          <h3 class="app__feature-option-title">{{ option.title }}</h3>
          <p class="app__feature-option-desc">{{ option.desc }}</p>
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { computed, ref } from "vue";
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';

const { t } = useI18n();

const router = useRouter();

const options = ref([
  {
    key: 'encode',
    title: 'app__feature-options.encode',
    desc: 'app__feature-options-desc.encode-desc'
  },
  {
    key: 'decode',
    title: 'app__feature-options.decode',
    desc: 'app__feature-options-desc.decode-desc'
  }
]);

const localizedOptions = computed(() => {
  return options.value.map(option => {
    return {
      key: option.key,
      title: t(option.title),
      desc: t(option.desc)
    };
  });
});

function onSubmit(key){
  router.push({ name: 'upload-file', params: { key } });
}
</script>

<style scoped>
@import "FeaturesSelector.css";
</style>