package au.edu.csu.pit;

import java.util.Scanner;

public class ComputePersonalTax {

    public static void main(String[] args){
        IncomeTax incomeTax = new IncomeTax();
        Scanner userInput = new Scanner(System.in);

        int filingStatus;
        int taxableIncome;
        double taxes;

        System.out.print("Enter the filing status: ");
        filingStatus = userInput.nextInt();

        System.out.print("Enter the taxable income: ");
        taxableIncome = userInput.nextInt();

        taxes = incomeTax.calculateTax(filingStatus,taxableIncome);

        System.out.println("Tax is: " + taxes);
    }
}
