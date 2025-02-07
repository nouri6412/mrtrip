<template>
  <div class="layout-wrapper">
    <Sidebar />
    <div class="layout-main">
      <RouterView />
    </div>
  </div>
</template>

<script>
import { RouterLink, RouterView } from 'vue-router'
import Sidebar from './components/Sidebar.vue';

export default {
  components: {
    Sidebar
  }
};
</script>

<style>
.layout-wrapper {
  display: flex;
  min-height: 100vh;
}

.layout-main {
  flex-grow: 1;
  padding: 1rem;
}
</style>