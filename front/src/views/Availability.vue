<template>
    <div>
        <Toast />
        <Card class="mb-2">
            <template #title>
                <div class="text-center">فرم جستجو</div>
            </template>
            <template #content>
                <div class="grid m-0 pr-3 pl-3">
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <Select v-model="selectedAirLine" :options="AirLineItems" optionLabel="label" class="w-full" />
                            <label for="AirlinID">شرکت هواپیمائی</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <InputNumber id="username" class="w-full" />
                            <label for="username">تعداد بزرگسال</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <InputNumber id="username" class="w-full" />
                            <label for="username">تعداد خردسال</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <InputNumber id="username" class="w-full" />
                            <label for="username">تعداد طفل</label>
                        </FloatLabel>
                    </div>
                    <div class="col-1 flex">
                        <div class="flex ml-2 align-items-center">
                            <Checkbox v-model="sourcechecked" binary class="ml-1" />
                            <label for="username">داخلی</label>
                        </div>
                    </div>
                    <div class="col-5">

                        <FloatLabel variant="on">

                            <AutoComplete class="w-full" v-model="selectedsourceCities" :suggestions="filteredsourceCities"
                                @complete="searchsourceCities" :virtualScrollerOptions="{ itemSize: 38 }" variant="filled"
                                optionLabel="label" dropdown />
                            <label for="username">شهر مبدا</label>
                        </FloatLabel>
                    </div>

                    <div class="col-1 flex">
                        <div class="flex ml-2 align-items-center">
                            <Checkbox v-model="destchecked" binary class="ml-1" />
                            <label for="username">داخلی</label>
                        </div>
                    </div>
                    <div class="col-5">

                        <FloatLabel variant="on">

                            <AutoComplete class="w-full" v-model="selecteddestCities" :suggestions="filtereddestCities"
                                @complete="searchdestCities" :virtualScrollerOptions="{ itemSize: 38 }" variant="filled"
                                optionLabel="label" dropdown />
                            <label for="username">شهر مقصد</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <DatePicker mode="single" :column="1" class="w-full pdp-input text-center" />
                            <label for="username">تاریخ</label>
                        </FloatLabel>
                    </div>

                </div>
                <div class="grid">
                    <ProgressSpinner style="width: 50px; height: 50px" v-if="loading" class="mt-2" />
                </div>
            </template>
        </Card>
        <DataTable class="border-round" :value="list" :paginator="true" :rows="10">
            <Column field="Airline" header="کد شرکت هواپیمایی"></Column>
            <Column field="Origin" header="مبدا"></Column>
            <Column field="Destination" header="مقصد"></Column>
            <Column field="DepartureDateTime" header="ساعت تاریخ پرواز"></Column>
            <Column field="ArrivalDateTime" header="ساعت پایان پرواز"></Column>
            <Column field="FlightNo" header="شماره پرواز"></Column>
            <Column field="AdultTotalPrices" header="قیمت بزرگسال"></Column>
            <Column field="Transit" header="وضعیت توقف"></Column>
            <Column field="FlightClasses" header="کلاس موجود"></Column>
            <Column field="ClassesStatus" header="وضعیت موجودی کلاس"></Column>
            <Column field="ClassRefundStatus" header="وضعیت استرداد کلاس"></Column>
            <Column field="AircraftTypeName" header="نوع هواپیما"></Column>
            <Column field="AircraftTypeCode" header="تایپ هواپیما"></Column>
            <Column field="CurrencyCode" header="کد ارز"></Column>

            <Column header="عملیات">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" class="p-button-rounded p-button-text p-button-warning"
                        @click="editUser(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="displayModal" header="رزرو" :modal="true">
            <div class="p-fluid">
                <div class="p-field">
                    <label for="name">نام</label>
                    <InputText id="name" v-model="selectedUser.name" />
                </div>
                <div class="p-field">
                    <label for="email">ایمیل</label>
                    <InputText id="email" v-model="selectedUser.email" />
                </div>
            </div>
            <template #footer>
                <Button label="لغو" icon="pi pi-times" class="p-button-text" @click="closeModal" />
                <Button :label="isEditMode ? 'ذخیره' : 'افزودن'" icon="pi pi-check" class="p-button-success"
                    @click="saveUser" />
            </template>
        </Dialog>

        <ConfirmDialog />
    </div>
</template>
  
<script>
import { ref } from "vue";
import { useToast } from 'primevue/usetoast';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import ConfirmDialog from 'primevue/confirmdialog';
import FloatLabel from 'primevue/floatlabel';
import Card from 'primevue/card';
import InputNumber from 'primevue/inputnumber';
import Select from 'primevue/select';
import DatePicker from '@alireza-ab/vue3-persian-datepicker';
import AutoComplete from 'primevue/autocomplete';
import Checkbox from 'primevue/checkbox';

import ProgressSpinner from 'primevue/progressspinner';
import Toast from 'primevue/toast';
import axios from "axios";
import { config } from "../../config"



// import { useConfirm } from 'primevue/useconfirm';

export default {
    components: {
        DataTable,
        Column,
        Button,
        Select,
        FloatLabel,
        InputNumber,
        Card,
        Dialog,
        InputText,
        DatePicker,
        AutoComplete,
        ConfirmDialog,
        Checkbox,
        Toast,
        ProgressSpinner
    },
    data() {
        return {
            list: [
            ],
            displayModal: false,
            selectedUser: {},
            isEditMode: false,
            cities: [
                { name: 'New York', code: 'NY' },
                { name: 'Rome', code: 'RM' },
                { name: 'London', code: 'LDN' },
                { name: 'Istanbul', code: 'IST' },
                { name: 'Paris', code: 'PRS' }
            ],
            sourceCities: [],
            destCities: [],

            selectedCity: null,
            date: '',
            selectedsourceCities: [],
            selecteddestCities: [],
            filteredsourceCities: [],
            filtereddestCities: [],
            sourcechecked: true,
            destchecked: true,
            selectedAirLine: null,
            AirLineItems: [],
            loading: false
        };
    },
    computed() {

    },
    mounted() {
        this.get_options('all',true)
    },
    watch: {
        sourcechecked() {
            console.log(this.sourcechecked)

            this.get_options( 'sourceCity',this.sourcechecked)

        },
        destchecked() {
            console.log(this.destchecked)

            this.get_options( 'destCity',this.destchecked)

        },
        selectedAirLine() {
            console.log(this.selectedAirLine)
        }
    },
    methods: {
        get_options(selectControl, checked) {
            this.loading = true;
          
            var token = localStorage.getItem("access_token");
            var obj = { methodID: "availabilityOptions", is_iran: checked ? 'is_iran' : 'not_iran', Authorization: token };

            if(selectControl=='all')
            {
                obj['get_airlines']=true;
                obj['get_cities']=true;
            }
            else{
                obj['get_cities']=true;
            }
            axios.post(`${config.app.api_url}`, obj, {
                headers: {
                    "Content-Type": "application/json"
                }
            })
                .then(response => {
                    console.log(response.data)
                    if (response.data.status == 1) {
                        if (selectControl == 'all') {
                            this.AirLineItems = response.data.airlines
                            this.sourceCities = response.data.cities
                            this.destCities = response.data.cities
                        }
                        else if (selectControl == 'sourceCity') {
                            this.sourceCities = response.data.cities
                        }
                        else if (selectControl == 'destCity') {
                            this.destCities = response.data.cities
                        }
                    }
                    else {
                        toast.add({
                            severity: 'error',
                            summary: 'خطا',
                            detail: 'خطا در بارگذاری اطلاعات لطفا دوباره امتحان فرمائید',
                            life: 3000,
                        });
                    }
                })
                .catch(error => {
                    toast.add({
                        severity: 'error',
                        summary: 'خطا',
                        detail: 'خطایی در سرور رخ داده است',
                        life: 3000,
                    });
                })
                .finally(() => {
                    this.loading = false
                })
        },
        searchsourceCities(event) {
            //in a real application, make a request to a remote url with the query and return filtered results, for demo we filter at client side
            let query = event.query;
            this.filteredsourceCities = this.sourceCities.filter(item => item.label.toLowerCase().includes(query.toLowerCase()))

        },
        searchdestCities(event) {
            //in a real application, make a request to a remote url with the query and return filtered results, for demo we filter at client side
            let query = event.query;
            this.filtereddestCities = this.destCities.filter(item => item.label.toLowerCase().includes(query.toLowerCase()))
        },
        openModal() {
            this.selectedUser = { id: null, name: '', email: '' };
            this.isEditMode = false;
            this.displayModal = true;
        },
        closeModal() {
            this.displayModal = false;
        },
        saveUser() {
            if (this.isEditMode) {
                // ویرایش کاربر
                const index = this.list.findIndex((u) => u.id === this.selectedUser.id);
                this.list.splice(index, 1, { ...this.selectedUser });
            } else {
                // افزودن کاربر جدید
                this.selectedUser.id = this.list.length + 1;
                this.list.push({ ...this.selectedUser });
            }
            this.closeModal();
        },
        editUser(user) {
            this.selectedUser = { ...user };
            this.isEditMode = true;
            this.displayModal = true;
        },
        deleteUser(user) {
            this.$confirm.require({
                message: 'آیا مطمئن هستید که می‌خواهید این کاربر را حذف کنید؟',
                header: 'تأیید حذف',
                icon: 'pi pi-exclamation-triangle',
                accept: () => {
                    this.list = this.list.filter((u) => u.id !== user.id);
                },
            });
        },
    },
    setup() {

        const toast = useToast();

        // const confirm = useConfirm();
        // return { confirm };
    },
};
</script>
  
<style scoped>
.p-field {
    margin-bottom: 1rem;
}
</style>
  
  