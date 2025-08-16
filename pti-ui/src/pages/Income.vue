<script setup lang="ts">
import { onMounted } from 'vue'
import { useTaxStore } from '@/stores/tax'
import IncomeForm from '@/components/IncomeForm.vue'
import IncomeTable from '@/components/IncomeTable.vue'

const store = useTaxStore()
onMounted(() => store.load())
</script>

<template>
  <h2>Income</h2>
  <IncomeForm />
  <IncomeTable :rows="store.income" />
</template>
