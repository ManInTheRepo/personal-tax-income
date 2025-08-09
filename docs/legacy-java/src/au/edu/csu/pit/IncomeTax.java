package au.edu.csu.pit;

import java.math.BigDecimal;
import java.math.RoundingMode;

/**
 *  class responsible for calculating the personal income tax
 */
public class IncomeTax {
    private final IncomeThresholds incomeThresholds;
    private int places = 2;

    public IncomeTax(){
        incomeThresholds = new IncomeThresholds();
    }

    public double calculateTax(int fillingStatus, double taxableIncome) {
        double tax = 0;
        double thresholdTaxIncome = 0;

        do {
            Threshold threshold = incomeThresholds.FindThresholdAmount(taxableIncome, fillingStatus);
            thresholdTaxIncome = taxableIncome - threshold.getAmount();
            /*  We calculate the taxable income that is applied in each threshold */
            tax += thresholdTaxIncome * threshold.getTaxRate();
            //decrement the total tax income by the tax income already taxed
            taxableIncome = taxableIncome - thresholdTaxIncome;
        } while (taxableIncome > 0);

        //To always have the tax to be rounding up to 2 decimal points.
        BigDecimal taxBigDecimal = BigDecimal.valueOf(tax).setScale(places, RoundingMode.HALF_UP);
        return taxBigDecimal.doubleValue();
    }
}