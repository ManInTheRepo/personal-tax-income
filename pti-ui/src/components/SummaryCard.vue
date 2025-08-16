<script setup lang="ts">
import { useTaxStore } from '@/stores/tax'
const store = useTaxStore()
</script>

<template>
  <div v-if="store.summary" style="border:1px solid #ddd; padding:12px; border-radius:8px; max-width:420px">
    <h3>Tax Summary</h3>
    <ul>
      <li>Taxable income: {{ store.summary.taxableIncome.toLocaleString() }}</li>
      <li>Estimated tax: {{ store.summary.estimatedTax.toLocaleString() }}</li>
      <li v-if="store.summary.medicareLevy !== undefined">Medicare levy: {{ store.summary.medicareLevy.toLocaleString() }}</li>
      <li><strong>Net income: {{ store.summary.netIncome.toLocaleString() }}</strong></li>
    </ul>
  </div>
  <p v-else>No summary yet — add income to calculate.</p>
</template>
