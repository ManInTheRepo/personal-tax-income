<script setup lang="ts">
import { onMounted } from 'vue';
import { useTaxStore } from '@/stores/tax';
import SummaryCard from '@/components/SummaryCard.vue';

const store = useTaxStore();
onMounted(() => store.refreshSummary());
</script>

<template>
  <h2>Summary</h2>
  <SummaryCard v-if="store.summary" :summary="store.summary" />
  <p v-else>Summary not available yet.</p>
</template>
