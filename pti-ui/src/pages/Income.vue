<script setup lang="ts">
import { onMounted } from 'vue';
import { useTaxStore } from '@/stores/tax';
import IncomeForm from '@/components/IncomeForm.vue';
import IncomeTable from '@/components/IncomeTable.vue';

const store = useTaxStore();
onMounted(() => { if (!store.income.length) store.load(); });
</script>

<template>
  <h2>Income</h2>
  <IncomeForm @submitted="store.addIncome" />
  <IncomeTable :rows="store.income" @delete="store.deleteIncome" />
</template>
