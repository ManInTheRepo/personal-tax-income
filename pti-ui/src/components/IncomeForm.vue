<script setup lang="ts">
import { reactive } from 'vue';
import type { IncomeItem } from '@/types/tax';

type Payload = Omit<IncomeItem, 'id'>;
const emit = defineEmits<{ (e: 'submitted', payload: Payload): void }>();

const form = reactive<Payload>({
  source: '',
  amount: 0,
  period: 'yearly',
  date: new Date().toISOString().slice(0,10)
});

function onSubmit() {
  if (!form.source || !form.amount) return;
  emit('submitted', { ...form });
  form.source = ''; form.amount = 0;
}
</script>

<template>
  <form @submit.prevent="onSubmit" style="display:grid; gap:8px; max-width:480px">
    <input placeholder="Source" v-model="form.source" />
    <input type="number" placeholder="Amount" v-model.number="form.amount" min="0" step="0.01" />
    <select v-model="form.period">
      <option value="weekly">Weekly</option>
      <option value="fortnightly">Fortnightly</option>
      <option value="monthly">Monthly</option>
      <option value="annual">Annual</option>
    </select>
    <input type="date" v-model="form.date" />
    <button type="submit">Add income</button>
  </form>
</template>
