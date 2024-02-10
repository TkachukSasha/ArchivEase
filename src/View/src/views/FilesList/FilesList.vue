<template>
  <div class="files__list-wrapper">
    <h1 class="files__not-found" v-if="items.length === 0">{{ $t('files.not-found') }}</h1>
    <ul class="files__list">
      <li class="files__list-item" v-for="(file, index) in items" :key="index">
        <div class="files__list-item-content">
          <h3 class="files__item-name">{{ file.fileName }}</h3>

          <div class="files__item-details">
            <span class="files__item-size">{{ file.defaultSize }} -> {{ file.encodedSize }}</span>
            <a :href="file.filePath" class="files__download-link">
              <font-awesome-icon icon="fa-solid fa-download" />
            </a>
          </div>
        </div>
      </li>
    </ul>
  </div>

  <Paginator
      :total-pages="totalElements"
      :currentPage="currentPage"
      :max-visible-buttons="3"
      @pageChanged="onPageChange"
  />
</template>

<script setup>
import { computed, onMounted, ref } from "vue";
import Paginator from "@/components/Paginator/Paginator.vue";
import { useFiles } from "./compositions/files";

const loaded = ref(false);
const currentPage = ref(1);
const perPage = ref(5);
const totalElements = ref(0);
const files = ref([]);

const items = computed(() => {
  let start = (currentPage.value - 1) * perPage.value,
      end = start + perPage.value;

  return files.value.slice(start, end);
});

async function onPageChange(page) {
  currentPage.value = page;

  const totalPages = Math.ceil(totalElements.value / perPage.value);

  if (page >= 1 && page <= totalPages) {
    const { response: fetchedFiles, loaded } = await useFiles({
      page: page,
      results: perPage.value,
    });

    loaded.value = loaded;

    files.value = fetchedFiles._value.items;
  }
}

onMounted(async () => {
  const { response: fetchedFiles, loaded } = await useFiles({
    page: 1,
    results: perPage.value,
  });

  files.value = fetchedFiles._value.items;

  loaded.value = loaded;

  totalElements.value = fetchedFiles._value.totalResults;
});
</script>

<style scoped>
@import "FilesList.css";
</style>