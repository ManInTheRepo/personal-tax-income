import { defineStore } from 'pinia';
import { Api } from '@/services/api';
import type { IncomeItem, TaxBreakDown } from '@/types/tax';

export const useTaxStore = defineStore('tax', {
  state: () => ({
    income: [] as IncomeItem[],
    summary: null as TaxBreakDown | null,
    loading: false,
    error: '' as string | '',
    count: 0
  }),
  actions: {
    async load() {
      this.loading = true; this.error = '';
      const [inc, sum] = await Promise.all([Api.listIncome(), Api.getSummary()]);
      if (inc.ok && sum.ok) {
        this.income = inc.data!;
        this.summary = sum.data!;
      } else {
        this.error = inc.error || sum.error || 'Failed to load';
      }
      this.loading = false;
    },
    async addIncome(item: Omit<IncomeItem, 'id'>) {
      const res = await Api.addIncome(item);
      if (res.ok && res.data) {
        this.income.push(res.data);
        await this.refreshSummary();
      } else {
        this.error = res.error || 'Failed to add income';
      }
    },
    async deleteIncome(id: string) {
      const res = await Api.deleteIncome(id);
      if (res.ok) {
        this.income = this.income.filter(x => x.id !== id);
        await this.refreshSummary();
      } else {
        this.error = res.error || 'Failed to delete';
      }
    },
    async refreshSummary() {
      const res = await Api.getSummary();
      if (res.ok) this.summary = res.data!;
    }
  }
});
