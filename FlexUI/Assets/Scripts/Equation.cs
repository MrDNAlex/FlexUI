using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlexUI
{
    public class Equation
    {
        //Equation
        public List<FlexPolynomial> cleanPoly = new List<FlexPolynomial>();

        public List<FlexPolynomial> polynomials;

        public Equation()
        {
            polynomials = new List<FlexPolynomial>();
        }

        public void addPolynomial(FlexPolynomial poly)
        {
            polynomials.Add(poly);
        }

        public void addPolynomial(float flex, float pow, string variable = "x")
        {
            FlexPolynomial poly = new FlexPolynomial(flex, pow, variable);

            polynomials.Add(poly);
        }

        public void derive()
        {
            //Add this for the polyclean aswell

            polyClean();

            foreach (FlexPolynomial i in cleanPoly)
            {
                i.derive();
            }

            polynomials = cleanPoly;
        }

        public Equation deriveWithRespect(string var)
        {
            Equation derEQ = new Equation();
            polyClean();
            foreach (FlexPolynomial i in cleanPoly)
            {

                Debug.Log(i.deriveWRespect(var).coefficient);

                derEQ.addPolynomial(i.deriveWRespect(var));
            }

            return derEQ;

        }


        //
        //Doesn't Apply
        //

        public float output(List<PolyOutput> vals)
        {

            polyClean();
            float value = 0;

            //Loop through to add the 
            //value = cleanPoly[0].output(0);

            //Check for power 0 first
            foreach (FlexPolynomial i in cleanPoly)
            {
                if (i.power == 0)
                {
                    value += i.coefficient;
                }
            }

            foreach (PolyOutput i in vals)
            {
                foreach (FlexPolynomial j in cleanPoly)
                {
                    if (j.getVariable() == i.variable)
                    {
                        // Debug.Log("Add: " + j.coefficient + " * " + j.variable + "^" + j.power + " (" + j.variable + "= " + i.value + ") = "+  j.output(i.value));
                        value += j.output(i.value);
                    }
                }
            }

            // Debug.Log("Output Value: " + value);
            return value;
        }

        //Add extra variables such as initial guess and stuff
        //Switch this to list of floats and also list of equations

        public float solveSingleEQ(float ans, string debugname = "")
        {
            polyClean();


           // Debug.Log(ans);
            //polyClean();

            int varNum = getVarCount();
           // Debug.Log("Var Count: " + getVarCount());

            // int varNum = getVarCount();

            Equation newEQ = new Equation();
            string var = getViableVariable(getVarList());
            Equation derivative = new Equation();
            float x = 1;
            float newX = 0;
            float diffPer = 0;

            float value = 0;
            switch (varNum)
            {
                case 0:
                    //Either only 0 degree polynomial or only x, x^2 ect

                    //Do I just return the value itself? yeah probably now that I think about it

                    value = ans;

                    break;
                case 1:

                    Debug.Log("Case 1");


                    //Ok gotta add the answer to the equation too as a negative aswell

                    //Ok let's make this like memory safe and shit

                    //One variable only, 

                    //Honestly just need to do newton raphson for this basically


                    newEQ.setEquation(cleanPoly);
                    newEQ.addPolynomial(new FlexPolynomial(-ans, 0, var));
                    newEQ.polyClean();

                    // Debug.Log("New Equation");
                   // newEQ.displayCleanPoly();


                    derivative.setEquation(cleanPoly);
                    derivative.derive();
                    derivative.polyClean();

                    // Debug.Log("Derivative");
                    // derivative.displayCleanPoly();

                    //loop for 100 or until change is under 0.1%

                    //X initial guess = 1



                    for (int i = 0; i < 100; i++)
                    {
                        List<PolyOutput> vals = new List<PolyOutput>();

                        vals.Add(new PolyOutput(var, x));

                        //Actually calculate the new X using Newton Raphson
                        newX = x - (newEQ.output(vals) / derivative.output(vals));


                        diffPer = Mathf.Abs((((float)newX - (float)x) / (float)newX) * 100);

                        x = newX;

                        // Debug.Log(diffPer);

                        if (diffPer < 0.001f)
                        {
                            i = 100;
                        }
                    }

                    Debug.Log("X " + x);
                    value = x;

                    break;

                case 2:

                    Debug.Log("Case 2");

                    //Ok gotta add the answer to the equation too as a negative aswell

                    //Ok let's make this like memory safe and shit

                    //One variable only, 

                    //Honestly just need to do newton raphson for this basically

                    // Equation newEQ = new Equation();
                    newEQ.setEquation(cleanPoly);
                    newEQ.addPolynomial(new FlexPolynomial(-ans, 0, var));
                    newEQ.polyClean();

                    Debug.Log(debugname);
                    newEQ.displayCleanPoly();
                    newEQ.displayAllPoly();
                    // Debug.Log("New Equation");
                    //newEQ.displayCleanPoly();


                    derivative.setEquation(cleanPoly);
                    derivative.derive();

                    // Debug.Log("Derivative");
                    // derivative.displayCleanPoly();

                    //loop for 100 or until change is under 0.1%

                    //X initial guess = 1



                    for (int i = 0; i < 100; i++)
                    {
                        List<PolyOutput> vals = new List<PolyOutput>();

                        vals.Add(new PolyOutput(var, x));

                        //Actually calculate the new X using Newton Raphson
                        newX = x - (newEQ.output(vals) / derivative.output(vals));


                        diffPer = Mathf.Abs((((float)newX - (float)x) / (float)newX) * 100);

                        x = newX;

                       

                        // Debug.Log(diffPer);

                        if (diffPer < 0.001f && i > 5)
                        {
                            i = 100;
                        }
                    }

                     //Debug.Log(x);
                  
                    value = x;


                    break;

            }

            return value;
        }


        /*
        public float solveX(float ans, List<Equation> eqList)
        {

            polyClean();

            //polyClean();

            int varNum = getVarCount();


           // int varNum = getVarCount();

            float value = 0;
            switch (varNum)
            {
                case 0:
                    //Either only 0 degree polynomial or only x, x^2 ect

                    //Do I just return the value itself? yeah probably now that I think about it

                    value = ans;

                    break;
                case 1:

                    //Ok gotta add the answer to the equation too as a negative aswell

                    //Ok let's make this like memory safe and shit


                    Equation newEQ = this;

                    //Idk fix this later
                    newEQ.addPolynomial2(-1 * ans, 0, polynomials[1].variable);

                    newEQ.polyClean();

                    //One variable only, 

                    //Honestly just need to do newton raphson for this basically

                    Equation derivative = new Equation();
                    derivative.setEquation(newEQ.cleanPoly);
                    derivative.derive();

                    //loop for 100 or until change is under 0.1%

                    //X initial guess = 1
                    float x = 1;
                    float newX = 0;
                    float diffPer = 0;


                    for (int i = 0; i < 100; i++)
                    {
                        List<PolyOutput> vals = new List<PolyOutput>();

                        vals.Add(new PolyOutput(newEQ.cleanPoly[1].variable, x));

                        //Actually calculate the new X using Newton Raphson
                        newX = x - (newEQ.output(vals) / derivative.output(vals));


                        diffPer = Mathf.Abs((((float)newX - (float)x) / (float)newX) * 100);

                        x = newX;

                       // Debug.Log(diffPer);

                        if (diffPer < 0.001f && i > 5)
                        {
                            i = 100;
                        }


                    }

                   // Debug.Log(x);
                    value = x;

                    break;
                default:
                    //For any other number of variables inside it 
                    //Will have to use either gauss Siedel Method, or Multivariable Newton Raphson
                    //I'm thinking we do multivariable Newton raphson, will be hella hard though 




                    break;
            }

            return value;
        }

        */
        public void polyClean()
        {
            cleanPoly = new List<FlexPolynomial>();

            //Kinda need a double dimension array

            //Save variable type and it's powers in order

            //displayAllPoly();


            foreach (FlexPolynomial i in polynomials)
            {
                if (!containsPoly(cleanPoly, i))
                {
                    cleanPoly.Add(new FlexPolynomial(0, i.power, i.getVariable()));
                }
            }



            /*
            List<float> pows = new List<float>();
            foreach (Polynomial i in polynomials)
            {
                if (!pows.Contains(i.power))
                {
                    pows.Add(i.power);
                }
            }

            pows.Sort();

            int variableNums = getVarCount();
            List<string> varNames = getVarList();

            cleanPoly.Clear();
            cleanPoly = new List<Polynomial>();

            cleanPoly.Add(new Polynomial(0, 0, ""));

            //Create every possible variable and power polynomials
            //Loop through every variable
            foreach (string var in varNames)
            {
                //Loop through every power to add them up
                foreach (float pow in pows)
                {





                    if (pow != 0)
                    {
                        cleanPoly.Add(new Polynomial(0, pow, var));
                    }
                }
            }

            */


            //Add the polynomials to the list

            foreach (FlexPolynomial i in cleanPoly)
            {
                foreach (FlexPolynomial j in polynomials)
                {
                    //Check if the power is 0, if it is add all the polynomials of power 0 into it since they would just be regular numbers that can be added
                    if (i.power == 0)
                    {
                        if (j.power == 0)
                        {
                            i.coefficient += j.coefficient;
                        }
                    }
                    else if (j.getVariable() == i.getVariable())
                    {
                        if (j.power == i.power)
                        {
                            i.coefficient += j.coefficient;
                        }
                    }
                }
            }

            // displayCleanPoly();


            /*
            //Gotta find a way to make this cleanup
            //clean up
            foreach (Polynomial2 i in cleanPoly)
            {
                if (i.coefficient == 0)
                {
                    cleanPoly.Remove(i);
                }
            }
            */

            polynomials = cleanPoly;

            //
            //Gotta check for the 0 powers, combine those into one polynomial 
            //

            //Maybe also have a cleanup section where it just loops and removes all the polynomaisl with coefficient 0?
        }

      

        //
        //Doesn't Apply
        //

        public float getMaxPow()
        {


            polyClean();
            float highPow = 0;
            foreach (FlexPolynomial i in cleanPoly)
            {
                if (i.power >= highPow)
                {
                    highPow = i.power;
                }
            }

            return highPow;

            //return cleanPoly[cleanPoly.Count - 1].power;
        }

        public int getVarCount()
        {
            List<string> varNames = new List<string>();

            foreach (FlexPolynomial i in polynomials)
            {
                if (!varNames.Contains(i.getVariable()))
                {
                    varNames.Add(i.getVariable());
                }
            }

            if (varNames.Count == 1)
            {
                if (varNames[0] == "" || varNames[0] == " " || varNames[0] == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return varNames.Count;
            }
        }

        public List<string> getVarList()
        {
            List<string> varNames = new List<string>();

            foreach (FlexPolynomial i in polynomials)
            {
                if (!varNames.Contains(i.getVariable()))
                {
                    varNames.Add(i.getVariable());
                }
            }

            return varNames;
        }

        //So to clean this equation up, we will need to check if the variable name is the same aswell as the powers

        public void setEquation(List<FlexPolynomial> eq)
        {
            this.polynomials = eq;
        }

        public bool containsPoly(FlexPolynomial poly)
        {
            foreach (FlexPolynomial i in polynomials)
            {
                if (i.getVariable() == poly.getVariable())
                {
                    if (i.power == poly.power)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public static bool containsPoly(List<FlexPolynomial> list, FlexPolynomial poly)
        {
            foreach (FlexPolynomial i in list)
            {
                if (i.getVariable() == poly.getVariable())
                {
                    if (i.power == poly.power)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public List<FlexPolynomial> getEQ()
        {
            //Actually we need to make a new thing for this
            List<FlexPolynomial> list = new List<FlexPolynomial>();

            foreach (FlexPolynomial i in polynomials)
            {
                FlexPolynomial newPoly = new FlexPolynomial(i.coefficient, i.power, i.getVariable());
                list.Add(newPoly);
            }



            return list;
        }

        public void displayAllPoly()
        {
            int count = 0;
            foreach (FlexPolynomial i in polynomials)
            {
                Debug.Log("Poly " + count + " : " + i.coefficient + i.getVariable() + " Pow : " + i.power);
                count++;
            }
        }

        public void displayCleanPoly()
        {
            int count = 0;
            foreach (FlexPolynomial i in cleanPoly)
            {
                Debug.Log("Poly " + count + " : " + i.coefficient + i.getVariable() + " Pow : " + i.power);
                count++;
            }
        }

        public void removePolynomial(FlexPolynomial poly)
        {
            polynomials.Remove(poly);

            polyClean();
        }

        //This one looks for a certain variable and power
        public void removePolynomial(float power, string var)
        {
            for (int i = 0; i < polynomials.Count; i++)
            {
                if (polynomials[i].getVariable() == var)
                {
                    if (polynomials[i].power == power)
                    {
                        polynomials.RemoveAt(i);
                    }
                }
            }
        }

        public string getViableVariable (List<string> vars)
        {
            string var = "";
            foreach (string i in vars)
            {
                if (i != "")
                {
                    var = i;
                }
            }
            if (var != "")
            {
                return var;
            } else
            {
                return "x";
            }
        } 



    }
}

