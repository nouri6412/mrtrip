<template>
    <div class="sidebar">
      <div class="logo">
        <i class="pi pi-home"></i>
        <span>داشبورد</span>
        <RouterLink to="/">داشبورد</RouterLink>

      </div>
      <nav>
        <RouterLink to="/users">کاربران</RouterLink>
      </nav>
    </div>
  </template>
  
  <script>
  import Menu from 'primevue/menu';
  import { RouterLink, RouterView } from 'vue-router'
  
  export default {
    components: {
      Menu
    },
    data() {
      return {
        items: [
          {
            label: 'لیست کاربران',
            icon: 'pi pi-users',
            to: '/users'
          },
          {
            label: 'تنظیمات',
            icon: 'pi pi-cog',
            to: '/settings'
          }
        ]
      };
    }
  };
  </script>
  
  <style>
  .sidebar {
    width: 250px;
    background-color: #f4f4f4;
    padding: 1rem;
  }
  
  .logo {
    display: flex;
    align-items: center;
    margin-bottom: 1rem;
  }
  
  .logo i {
    margin-left: 0.5rem;
  }
  </style>