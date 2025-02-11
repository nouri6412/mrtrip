import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/Home.vue'


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
    ,
    {
      path: '/availability',
      name: 'availability',
      component: () => import('../views/Availability.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/Login.vue'),
    }
  ],
})

export default router
