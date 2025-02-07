<template>
  <div class="sidebar" :class="{ 'sidebar-collapsed': isCollapsed }">
    <Button
      icon="pi pi-bars"
      class="p-button-text sidebar-toggle"
      @click="toggleSidebar"
    />
    <div class="logo">
      <i class="pi pi-home"></i>
      <span v-if="!isCollapsed">داشبورد</span>
    </div>
    <nav>
        <RouterLink to="/users">کاربران</RouterLink>
      </nav>
  </div>
</template>

<script>
import Menu from 'primevue/menu';
import Button from 'primevue/button';
import { RouterLink, RouterView } from 'vue-router'

export default {
  components: {
    Menu,
    Button,
  },
  data() {
    return {
      isCollapsed: false,
      items: [
        {
          label: 'لیست کاربران',
          icon: 'pi pi-users',
          to: '/users',
        },
        {
          label: 'تنظیمات',
          icon: 'pi pi-cog',
          to: '/settings',
        },
      ],
    };
  },
  methods: {
    toggleSidebar() {
      this.isCollapsed = !this.isCollapsed;
    },
  },
};
</script>

<style scoped>
.sidebar {
  width: 250px;
  background-color: #f4f4f4;
  padding: 1rem;
  transition: width 0.3s;
}

.sidebar-collapsed {
  width: 60px;
}

.sidebar-toggle {
  position: absolute;
  right: -40px;
  top: 10px;
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