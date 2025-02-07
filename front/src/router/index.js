import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/Users.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/users',
      name: 'users',
      component: () => import('../views/Users.vue'),
    }
  ],
})

export default router
