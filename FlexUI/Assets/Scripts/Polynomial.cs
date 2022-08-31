using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlexUI
{

    [System.Serializable]
    public class FlexPolynomial
    {
        
        public float coefficient;
        public float power;
        string variable = "x"; //Default Variable is x 


        public FlexPolynomial(float coef, float pow, string variable = "x")
        {
            //Coefficient
            //Power
            this.coefficient = coef;
            this.power = pow;

            if (variable != null)
            {
                this.variable = variable;
            }
            else if (pow >= 1)
            {
                this.variable = "x"; //First variable type
            }

            if (power == 0)
            {
                this.variable = "";
            }

        }

        //Make a function to return output (when x input), func for derivative

        public float output(float x)
        {
            return coefficient * Mathf.Pow(x, power);
        }

        public void derive()
        {
            this.coefficient = coefficient * power;
            this.power = this.power - 1;
        }

        public FlexPolynomial deriveWRespect(string var)
        {
            if (var == this.variable)
            {
                return new FlexPolynomial(coefficient * power, power - 1, variable);
            }
            else
            {

                return new FlexPolynomial(0, power, variable);
            }
        }

        public string getVariable ()
        {
            return variable;
        }

        public FlexPolynomial getOppositePoly ()
        {
            Debug.Log(variable);
            return new FlexPolynomial(coefficient * -1, power, variable);
        }

        public void setVariable (string var)
        {
            variable = var;
        }


    }

}

