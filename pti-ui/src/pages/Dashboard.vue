<script setup lang="ts">
import { onMounted } from 'vue';
import { useTaxStore } from '@/stores/tax';
import SummaryCard from '@/components/SummaryCard.vue';

const store = useTaxStore();
onMounted(() => { if (!store.summary) store.load(); });
</script>

<template>
  <div>
    <h2>Dashboard</h2>
    <p v-if="store.loading">Loading…</p>
    <p v-else-if="store.error">Error: {{ store.error }}</p>
    <SummaryCard v-else-if="store.summary" :summary="store.summary" />
    <div v-else>No data yet. Add income to get started.</div>
  </div>
</template>
