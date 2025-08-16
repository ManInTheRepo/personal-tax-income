import { setActivePinia, createPinia } from 'pinia'
import { useTaxStore } from '@/stores/tax'
import { beforeEach, expect, test } from 'vitest'

beforeEach(() => {
  setActivePinia(createPinia())
  localStorage.clear()
})

test('adds income and recomputes summary', () => {
  const store = useTaxStore()
  store.addIncome({ source: 'Employer', amount: 100000, period: 'yearly', date: '2025-07-01' })
  expect(store.count).toBe(1)
  expect(store.income).toEqual([{ source: 'Employer', amount: 100000, period: 'yearly', date: '2025-07-01' }])
  expect(store.summary?.estimatedTax).toBe(Math.round(100000 * 0.25))
})
