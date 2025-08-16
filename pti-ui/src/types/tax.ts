export interface IncomeItem {
    id: string;
    source: string;
    amount: number;
    period?: 'weekly' | 'fortnightly' | 'monthly' | 'yearly';
    date?: string
}

export interface TaxBreakDown {
    taxableIncome: number;
    estimatedTax: number;
    medicareLevy: number;
    netIncome: number;
}

export interface ApiResult<T> {
    ok: boolean;
    data?: T;
    error?: string;
}
