<script setup lang="ts">
import type { TaxBreakDown } from '@/types/tax';
const props = defineProps<{ summary: TaxBreakDown }>();
</script>

<template>
  <div style="border:1px solid #ddd; padding:12px; border-radius:8px; max-width:420px">
    <h3>Tax Summary</h3>
    <ul>
      <li>Taxable income: {{ props.summary.taxableIncome.toLocaleString() }}</li>
      <li>Estimated tax: {{ props.summary.estimatedTax.toLocaleString() }}</li>
      <li v-if="props.summary.medicareLevy !== undefined">Medicare levy: {{ props.summary.medicareLevy.toLocaleString() }}</li>
      <li><strong>Net income: {{ props.summary.netIncome.toLocaleString() }}</strong></li>
    </ul>
  </div>
</template>
