<template>
  <div>
    <Button
      label="افزودن کاربر"
      icon="pi pi-plus"
      class="p-button-success"
      @click="openModal"
    />

    <DataTable :value="users" :paginator="true" :rows="10">
      <Column field="id" header="ID"></Column>
      <Column field="name" header="نام"></Column>
      <Column field="email" header="ایمیل"></Column>
      <Column header="عملیات">
        <template #body="slotProps">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-text p-button-warning"
            @click="editUser(slotProps.data)"
          />
          <Button
            icon="pi pi-trash"
            class="p-button-rounded p-button-text p-button-danger"
            @click="deleteUser(slotProps.data)"
          />
        </template>
      </Column>
    </DataTable>

    <Dialog
      v-model:visible="displayModal"
      :header="isEditMode ? 'ویرایش کاربر' : 'افزودن کاربر'"
      :modal="true"
    >
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
        <Button
          label="لغو"
          icon="pi pi-times"
          class="p-button-text"
          @click="closeModal"
        />
        <Button
          :label="isEditMode ? 'ذخیره' : 'افزودن'"
          icon="pi pi-check"
          class="p-button-success"
          @click="saveUser"
        />
      </template>
    </Dialog>

    <ConfirmDialog />
  </div>
</template>

<script>
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import ConfirmDialog from 'primevue/confirmdialog';
// import { useConfirm } from 'primevue/useconfirm';

export default {
  components: {
    DataTable,
    Column,
    Button,
    Dialog,
    InputText,
    ConfirmDialog,
  },
  data() {
    return {
      users: [
        { id: 1, name: 'علی', email: 'ali@example.com' },
        { id: 2, name: 'رضا', email: 'reza@example.com' },
      ],
      displayModal: false,
      selectedUser: { id: null, name: '', email: '' },
      isEditMode: false,
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
        const index = this.users.findIndex((u) => u.id === this.selectedUser.id);
        this.users.splice(index, 1, { ...this.selectedUser });
      } else {
        // افزودن کاربر جدید
        this.selectedUser.id = this.users.length + 1;
        this.users.push({ ...this.selectedUser });
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
          this.users = this.users.filter((u) => u.id !== user.id);
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