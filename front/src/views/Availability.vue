<template>
    <div>
        <Card class="mb-2">
            <template #title>
                <div class="text-center">فرم جستجو</div>
            </template>
            <template #content>
                <div class="grid m-0 pr-3 pl-3">
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <InputNumber id="username" class="w-full" />
                            <label for="username">تعداد بزرگسال</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <FloatLabel variant="on">
                            <Select v-model="selectedCity" :options="cities" optionLabel="name" class="w-full" />
                            <label for="username">شهر مبدا</label>
                        </FloatLabel>
                    </div>
                    <div class="col-3">
                        <!-- <label for="username">تاریخ پرواز</label> -->
                        <DatePicker  class="w-full"/>
                    </div>
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
        ConfirmDialog
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
            selectedCity: null,
            date: ''
        };
    },
    methods: {
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
  
  