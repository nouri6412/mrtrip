<template>
  <div class=" flex justify-content-center align-items-center" v-if="loading">
    <ProgressSpinner class="mt-4" />
  </div>
  <div v-if="!loading" class="layout-wrapper">
    <Sidebar v-if="isLogin" />

    <div class="layout-main">

      <RouterView />
    </div>
  </div>
</template>

<script>
import { config } from "../config"
import { RouterLink, RouterView } from 'vue-router'
import Sidebar from './components/Sidebar.vue';

import { ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useRouter } from 'vue-router';
import ProgressSpinner from 'primevue/progressspinner';
import Toast from 'primevue/toast';
import axios from "axios";

export default {
  components: {
    Sidebar,
    ProgressSpinner,
    Toast,
  },
  setup() {
    const loading = ref(true);
    const isLogin = ref(false);
    const toast = useToast();
    const router = useRouter();
    const get_data = () => {
      loading.value = true;
      const token = localStorage.getItem("access_token");
      const obj = { methodID: "get_home",Authorization:token };

      let hasError = true;

      axios.post(`${config.app.api_url}`, obj, {
        headers: {
          "Content-Type": "application/json"
        }
      })
        .then(response => {
          console.log(response.data)
          if (response.data.status == 1) {
            isLogin.value=true;
          }
          else{
           // localStorage.setItem('access_token', '')
           router.push('/login')
          }
         
        })
        .catch(error => {
          hasError = true
          toast.add({
            severity: 'error',
            summary: 'خطا',
            detail: 'خطایی در سرور رخ داده است',
            life: 3000,
          });
        })
        .finally(() => {
          loading.value = false
        })
    };
    get_data();
    return {
      loading,
      isLogin,
      get_data,
    };
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