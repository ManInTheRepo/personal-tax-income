import type { ApiResult, IncomeItem, TaxBreakDown } from "../types/tax";
const BASE = import.meta.env.VITE_API_BASE_URL ?? '/api';

async function json<T>(res: Response): Promise<ApiResult<T>> {
  if (!res.ok) {
    return { ok: false, error: `${res.status} ${res.statusText}` };
  }
  try {
    const data = await res.json() as T;
    return { ok: true, data };
  } catch {
    return { ok: false, error: 'Invalid JSON' };
  }
}

export const Api = {
  async listIncome(): Promise<ApiResult<IncomeItem[]>> {
    return json<IncomeItem[]>(await fetch(`${BASE}/income`));
  },
  async addIncome(item: Omit<IncomeItem, 'id'>): Promise<ApiResult<IncomeItem>> {
    return json<IncomeItem>(await fetch(`${BASE}/income`, {
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(item)
    }));
  },
  async deleteIncome(id: string): Promise<ApiResult<{ id: string }>> {
    return json(await fetch(`${BASE}/income/${id}`, { method: 'DELETE' }));
  },
  async getSummary(): Promise<ApiResult<TaxBreakDown>> {
    return json<TaxBreakDown>(await fetch(`${BASE}/summary`));
  }
};