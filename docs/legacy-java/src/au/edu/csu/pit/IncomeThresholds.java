package au.edu.csu.pit;

/**
 *  This class represent the different Income Thresholds allowing access to the different
 *  tax threshold depending on the income
 */
public class IncomeThresholds {

    private double [][][] incomeThresholds;
    private int Threshold_LowerLimit = 0;
    private int Threshold_UpperLimit= 1;

    private int Tax_Rate = 4; //column number where tax rate is stored in the threshold

    public IncomeThresholds(){
        incomeThresholds = createThresholds();
    }

    private double[][][] createThresholds() {
        return  new double[][][]  {
                {
                        {0,8350}, {0,16700}, {0,8350}, {0, 11950},  {0.10}  //first row
                },
                {
                        {8351,33950}, {16701,67900}, {8351,33950}, {11951,45500}, {0.15} //second row
                } ,
                {
                        {33951,82250}, {67900,137050}, {33951, 68525} ,{45501, 117450}, {0.25}  //third row
                } ,
                {
                        {82251,171550} , {137051,208850} , {68526,104425}, {117451,190200}, {0.28} //fourth row
                },
                {
                        {171550,372950}, {208851,372950} , {104426,186475} ,{190201,372950}, {0.33}  //fifth row
                } ,
                {
                        {372951}, {372951}, {186476}, {372951}, {0.35} //sixth row
                }
        };
    }

    public Threshold FindThresholdAmount(double income, int filersStatus) {
        Threshold threshold = null;
        int numberOfThresholds = this.incomeThresholds.length;

        for (int currentThreshold = 0; currentThreshold < numberOfThresholds; currentThreshold++){
            double [] thresholds =  this.incomeThresholds[currentThreshold][filersStatus];
            double [] taxRate = this.incomeThresholds[currentThreshold][Tax_Rate];

            /*  It handles incomes that falls onto the top tax rate threshold */
            if (thresholds.length == 1 && income >= thresholds[0] ){
                return new Threshold(incomeThresholds[currentThreshold-1][filersStatus][Threshold_UpperLimit]
                                    ,taxRate[0]);
            }

            if(thresholds[Threshold_LowerLimit] <= income && thresholds[Threshold_UpperLimit] >= income
                            && currentThreshold > 0 )
                return new Threshold(this.incomeThresholds[currentThreshold-1][filersStatus][Threshold_UpperLimit],
                                        taxRate[0]);
        }
        //We have reached the lowest threshold that can be apply to a given tax income
        return new Threshold(0,0.10);
    }
}


