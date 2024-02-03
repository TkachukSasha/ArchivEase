import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: "main",
      path: "/",
      component: () => import('../views/Main.vue')
    },
    {
      name: "sign-in",
      path: "/sign-in",
      component: () => import('../views/SignIn/SignIn.vue')
    },
    {
      name: "sign-up",
      path: "/sign-up",
      component: () => import('../views/SignUp/SignUp.vue')
    },
    {
      name: "upload-file",
      path: "/:key",
      component: () => import('../views/FilesUploader/FilesUploader.vue')
    },
    {
      name: "files",
      path: "/files",
      component: () => import('../views/FilesList/FilesList.vue')
    },
    {
      name: "users",
      path: "/users",
      component: () => import('../views/UsersList/UsersList.vue')
    }
  ]
})

export default router