<script setup>
import { RouterLink, RouterView } from 'vue-router'
import Header from './views/Header.vue'
import SideBar from './views/SideBar.vue'

</script>

<template>
  <div class="app-container m-0 p-0">
    <div class="app-aside">
      <SideBar />
    </div>
    <div class="app-content">
      <div class="grid m-0 pr-3 pl-3">
        <div class="col-12 md:col-12 lg:col-12">
          <Header />
        </div>
        <div class="col-12 md:col-12 lg:col-12">
          <RouterView />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.app-container {
  display: flex;
  flex-wrap: wrap;
}

.app-content {
  min-height: 200px;
  width: 89%;
}

.app-aside {
  border-radius: var(--radius-large);
  background-color: var(--color-secondry);
  min-height: 200px;
  width: 11%;
  border: 1.5px solid var(--color-green);
}

@media (max-width: 767px) {
  .app-content {
    min-height: 200px;
    width: 100%;
  }

  .app-aside {
    display: none;
  }
}
</style>
