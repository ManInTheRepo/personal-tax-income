<script setup lang="ts">
import type { IncomeItem } from '@/types/tax';
const props = defineProps<{ rows: IncomeItem[] }>();
const emit = defineEmits<{ (e: 'delete', id: string): void }>();
</script>

<template>
  <table border="1" cellpadding="6" cellspacing="0">
    <thead>
      <tr>
        <th>Source</th><th>Amount</th><th>Period</th><th>Date</th><th></th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="r in props.rows" :key="r.id">
        <td>{{ r.source }}</td>
        <td>{{ r.amount.toLocaleString(undefined,{minimumFractionDigits:2}) }}</td>
        <td>{{ r.period ?? 'annual' }}</td>
        <td>{{ r.date ?? '' }}</td>
        <td><button @click="emit('delete', r.id)">Delete</button></td>
      </tr>
      <tr v-if="!props.rows.length">
        <td colspan="5">No income added yet.</td>
      </tr>
    </tbody>
  </table>
</template>
