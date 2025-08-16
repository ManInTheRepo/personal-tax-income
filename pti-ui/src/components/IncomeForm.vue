<script setup lang="ts">
import { reactive } from 'vue'
import { useTaxStore} from '@/stores/tax'

const store = useTaxStore()

type Period = 'yearly' | 'weekly' | 'fortnightly' | 'monthly';

const form = reactive<{
  source: string;
  amount: number;
  period: Period;
  date: string;
}>({
  source: '',
  amount: 0,
  period: 'yearly',
  date: new Date().toISOString().slice(0, 10)
})

function submit() {
  if (!form.source || form.amount <= 0) return
  store.addIncome({ ...form }) // add income to store
  form.source = ''; form.amount = 0
}
</script>

<template>
  <form @submit.prevent="submit" style="display:grid; gap:8px; max-width:480px">
    <input placeholder="Source" v-model="form.source" />
    <input type="number" placeholder="Amount" v-model.number="form.amount" min="0" step="0.01" />
    <select v-model="form.period">
      <option value="weekly">Weekly</option>
      <option value="fortnightly">Fortnightly</option>
      <option value="monthly">Monthly</option>
      <option value="yearly">Yearly</option>
    </select>
    <input type="date" v-model="form.date" />
    <button type="submit">Add income</button>
  </form>
</template>
