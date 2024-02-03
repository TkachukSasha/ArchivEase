<template>
  <div class="users__list-wrapper">
    <ul class="users__list">
      <li class="users__list-item" v-for="(user, index) in items" :key="index">
        <div class="users__list-item-content">
          <h3 class="users__item-user_name">{{ user.userName }}</h3>
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
import { useUsers } from "./compositions/users.js";

const currentPage = ref(1);
const perPage = ref(5);
const totalElements = ref(0);
const users = ref([]);

const items = computed(() => {
  let start = (currentPage.value - 1) * perPage.value,
      end = start + perPage.value;

  return users.value.slice(start, end);
});

async function onPageChange(page) {
  currentPage.value = page;

  const totalPages = Math.ceil(totalElements.value / perPage.value);

  if (page >= 1 && page <= totalPages) {
    const { response: fetchedUsers } = await useUsers({
      page: page,
      results: perPage.value,
    });

    users.value = fetchedUsers._value.items;
  }
}

onMounted(async () => {
  const { users: fetchedUsers } = await useUsers({
    page: 1,
    results: perPage.value,
  });

  users.value = fetchedUsers._value.items;

  totalElements.value = fetchedUsers._value.totalResults;
});
</script>

<style scoped>
@import "UsersList.css";
</style>