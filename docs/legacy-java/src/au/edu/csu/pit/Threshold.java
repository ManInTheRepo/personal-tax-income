package au.edu.csu.pit;

/**
 *  class that represents a single tax threshold
 */
public class Threshold {
    private double amount;
    private double taxRate;

    public Threshold(double amount, double taxRate){
        this.amount = amount;
        this.taxRate = taxRate;
    }

    public double getAmount() {
        return amount;
    }

    public double getTaxRate() {
        return taxRate;
    }
}
