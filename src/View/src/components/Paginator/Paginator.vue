<template>
  <ul class="paginator__items" v-if="totalPages > 1">
    <li class="paginator__item">
      <button type="button" @click="onClickFirstPage" :disabled="isInFirstPage">
        <font-awesome-icon icon="fa-solid fa-chevron-left" />
        <font-awesome-icon icon="fa-solid fa-chevron-left" />
      </button>
    </li>

    <li class="paginator__item">
      <button type="button" @click="onClickPreviousPage" :disabled="isInFirstPage">
        <font-awesome-icon icon="fa-solid fa-chevron-left" />
      </button>
    </li>

    <li class="paginator__item" v-for="(page, index) in pages" :key="index">
      <button
          type="button"
          @click="onClickPage(page.number)"
          :disabled="page.isDisabled"
          :class="{ active: isPageActive(page.number) }"
      >
        {{ page.number }}
      </button>
    </li>

    <li class="paginator__item">
      <button type="button" @click="onClickNextPage" :disabled="isInLastPage">
        <font-awesome-icon icon="fa-solid fa-chevron-right" />
      </button>
    </li>
    <li class="paginator__item">
      <button type="button" @click="onClickLastPage" :disabled="isInLastPage">
        <font-awesome-icon icon="fa-solid fa-chevron-right" />
        <font-awesome-icon icon="fa-solid fa-chevron-right" />
      </button>
    </li>
  </ul>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps(["maxVisibleButtons", "totalPages", "currentPage"]);
const emit = defineEmits(["pageChanged"]);

const isInFirstPage = computed(() => props.currentPage === 1);
const isInLastPage = computed(() => props.currentPage === props.totalPages);

const startPage = computed(() => {
  if (props.currentPage === 1) return 1;
  if (props.currentPage === props.totalPages)
    return props.totalPages - props.maxVisibleButtons + props.maxVisibleButtons - 1;
  return props.currentPage - 1;
});

const endPage = computed(() =>
    Math.min(startPage.value + props.maxVisibleButtons - 1, props.totalPages)
);

const pages = computed(() => {
  let range = [];
  for (let i = startPage.value; i <= endPage.value; i += 1) {
    range.push({
      number: i,
      isDisabled: i === props.currentPage,
    });
  }
  return range;
});

const onClickFirstPage = () => emit("pageChanged", 1);
const onClickPreviousPage = () => emit("pageChanged", props.currentPage - 1);
const onClickPage = (page) => emit("pageChanged", page);
const onClickNextPage = () => emit("pageChanged", props.currentPage + 1);
const onClickLastPage = () => emit("pageChanged", props.totalPages);

const isPageActive = (page) => props.currentPage === page;
</script>

<style scoped>
@import "Paginator.css";
</style>