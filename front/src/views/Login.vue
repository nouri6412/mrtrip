<template>
    <div class="flex justify-content-center align-items-center h-screen">
        <Card class="w-20rem">
            <template #title>
                <div class="text-center text-2xl font-bold">ورود به سیستم</div>
            </template>
            <template #content>
                <form @submit.prevent="handleSubmit">
                    <div class="field mt-4">
                        <label for="username" class="block text-right font-medium mb-2">نام کاربری</label>
                        <InputText id="username" v-model="username" :class="{ 'p-invalid': usernameError }"
                            placeholder="نام کاربری خود را وارد کنید" />
                        <small v-if="usernameError" class="p-error block text-right">{{ usernameError }}</small>
                    </div>
                    <div class="field mt-4">
                        <label for="password" class="block text-right font-medium mb-2">رمز عبور</label>
                        <Password id="password" v-model="password" :class="{ 'p-invalid': passwordError }"
                            placeholder="رمز عبور خود را وارد کنید" :feedback="false" />
                        <small v-if="passwordError" class="p-error block text-right">{{ passwordError }}</small>
                    </div>
                    <Button type="submit" label="ورود" class="w-full mt-4" :disabled="loading" />
                    <ProgressSpinner v-if="loading" class="mt-4" />
                </form>
            </template>
        </Card>
        <Toast />
    </div>
</template>
  
<script>
import { ref } from 'vue';
import { Form } from '@primevue/forms';
import { useToast } from 'primevue/usetoast';
import { useRouter } from 'vue-router';
import Card from 'primevue/card';
import InputText from 'primevue/inputtext';

import Password from 'primevue/password';
import Button from 'primevue/button';
import ProgressSpinner from 'primevue/progressspinner';
import Toast from 'primevue/toast';
import axios from "axios";
import { config } from "../../config"

export default {
    components: {
        Card,
        InputText,
        Password,
        Button,
        ProgressSpinner,
        Toast,
    },
    setup() {
        const username = ref('');
        const password = ref('');
        const usernameError = ref('');
        const passwordError = ref('');
        const loading = ref(false);
        const toast = useToast();
        const router = useRouter();

        const validateForm = () => {
            let isValid = true;

            if (!username.value) {
                usernameError.value = 'نام کاربری الزامی است.';
                isValid = false;
            } else {
                usernameError.value = '';
            }

            if (!password.value) {
                passwordError.value = 'رمز عبور الزامی است.';
                isValid = false;
            } else {
                passwordError.value = '';
            }

            return isValid;
        };
        const handleSubmit = () => {
            if (validateForm()) {
                loading.value = true;
                const obj = { username: username.value, password: password.value, methodID: "login" };

                let hasError = true;
                axios.post(`${config.app.api_url}`, obj, {
                    headers: {
                        "Content-Type": "application/json"
                    }
                })
                    .then(response => {
                        console.log(response.data)
                        hasError = response.data.status < 1;
                        if (response.data.status == 1) {
                            localStorage.setItem('access_token', response.data.token)
                            router.push('/')
                        }
                        else {
                            toast.add({
                                severity: 'error',
                                summary: 'خطا',
                                detail: 'نام کاربری و یا رمز عبور اشتباه است',
                                life: 3000,
                            });
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
            }
        };

        return {
            username,
            password,
            usernameError,
            passwordError,
            loading,
            handleSubmit,
        };
    },
};
</script>
  
<style scoped>
.h-screen {
    height: 100vh;
}

.w-30rem {
    width: 30rem;
}

.text-right {
    text-align: right;
}
</style>