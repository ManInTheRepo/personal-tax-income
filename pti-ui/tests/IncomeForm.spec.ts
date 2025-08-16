import { render, fireEvent } from '@testing-library/vue';
import { test, expect } from 'vitest';
import IncomeForm from '@/components/IncomeForm.vue';

test('submits a new income entry', async () => {
  const { getByPlaceholderText, getByText, emitted } = render(IncomeForm);

  await fireEvent.update(getByPlaceholderText('Source'), 'Employer');
  await fireEvent.update(getByPlaceholderText('Amount'), '100000');
  await fireEvent.click(getByText('Add income'));

  // assert emitted event
  expect(emitted()).toHaveProperty('submitted');
  const submittedEvents = emitted().submitted as any[];
  expect(submittedEvents[0][0]).toEqual({
    source: 'Employer',
    amount: 100000,
    period: 'yearly',
    date: expect.any(String)
  });
});
