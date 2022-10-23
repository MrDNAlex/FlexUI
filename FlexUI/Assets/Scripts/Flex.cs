using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FlexUI
{
    [System.Serializable]
    // [RequireComponent(typeof(RectTransform))]
    public class Flex 
    {


        //Develop a animation framework that calculates the first and last frames of the animation and then it loops through the animation instead of recalculating UI every frame


        // Rules

        //Flex Polynomial
        //When Assigning a Flex Polynomial with a Coefficient, it will reflect that number for size, padding, spacing and more
        //At Power 0 the value will be x Pixels
        //At Power 1 the value of one single unit of X in pixels will be calculated as the UI is initialized 

        //Reference to the RectTransform this Flex will modify


        RectTransform UI;

        //Reference to all the children Flexes 
        List<Flex> children = new List<Flex>();


        [Header("Flex")]
        public string name;
        //Flex value for the parent, (Cannot be turned into a pixel value, will only serve as a flex unit)
        public float flex;

        [Header("Self Padding")]
        //Applies to self (Not Children)
        public FlexPolynomial selfPadTopFlex;
        public FlexPolynomial selfPadBotFlex;
        public FlexPolynomial selfPadLeftFlex;
        public FlexPolynomial selfPadRightFlex;

        [Header("Padding")]
        //Applies only to objects Underneath (Children)
        public FlexPolynomial padTopFlex;
        public FlexPolynomial padBotFlex;
        public FlexPolynomial padLeftFlex;
        public FlexPolynomial padRightFlex;

        [Header("Spacing")]
        //Spacing between the objects children
        public FlexPolynomial spacingFlex;

        //Need to create functions to get these items if people want 

        [Header("Child Based Size Settings")]
        //A bool to see if it will use one of these values
        public bool useChildMulti;
        //A value that will determine the width of the Flex depending on number of children
        public float childMultiW;
        //A value that will determine the height of the Flex depending on number of children
        public float childMultiH;

        //All the Miscellanious settings that the flex can have 
        [Header("Misc Settings")]
        // Will make the flex a square dimension by looking at the cross axis size (Layout Group Horizontal : heightXheight, Layout Group Vertical : widthXwidth)
        public bool square;
        // Will fill in to the max dimensions of this flex's parent, doesn't work with vertical or horizontal layouts
        public bool fillParent;
        // Once clicked, the system will ignore modifying the flex
        public bool dontModify;
        // WIP, will allow you to input a custom size as a child
        public bool customDim;


      

        //Booleans to tell the program if there is a layoutgroup, and if it is Vertical or Horizontal
        bool layoutGroup;
        bool layoutGroupVert;

        //Solved values for a single Flex Unit to determine the Horizontal Flex Unit and Vertical Flex Unit value
        float hVal;
        float wVal;

        [Header("Starting Parent Settings")]
        //The size that the flex will start at
        public Vector2 size;
        [Header("Override Size")]
        //The size that a child flex will take if custom size toggle is on (WIP)
        public Vector2 customSize;

        public Flex(RectTransform UIItem, float flex)
        {
            // Debug.Log(UIItem);
            // Debug.Log(UIItem.sizeDelta);
            Debug.Log("Init");

            this.UI = UIItem;
            this.flex = flex;

            this.size = UIItem.sizeDelta;

            children = new List<Flex>();

            if (UIItem.gameObject.GetComponent<HorizontalLayoutGroup>())
            {
                layoutGroup = true;
                layoutGroupVert = false;
            }
            else if (UIItem.gameObject.GetComponent<VerticalLayoutGroup>())
            {
                layoutGroup = true;
                layoutGroupVert = true;
            }
            else
            {
                layoutGroup = false;
            }
        }

        public void addChild(Flex child)
        {
           
            children.Add(child);
        }

        public void setSpacingFlex(FlexPolynomial flex)
        {
            spacingFlex = flex;
        }

        public void setSpacingFlex(float coef, float power)
        {
            spacingFlex = new FlexPolynomial(coef, power);
        }

        public void setHorizontalPadding(FlexPolynomial leftFlex, FlexPolynomial rightFlex)
        {
            padLeftFlex = leftFlex;
            padRightFlex = rightFlex;
        }

        public void setHorizontalPadding(float leftCoef, float leftPow, float rightCoef, float rightPow)
        {
            padLeftFlex = new FlexPolynomial(leftCoef, leftPow);
            padRightFlex = new FlexPolynomial(rightCoef, rightPow);
        }

        public void setVerticalPadding(FlexPolynomial upFlex, FlexPolynomial downFlex)
        {
            padTopFlex = upFlex;
            padBotFlex = downFlex;
        }

        public void setVerticalPadding(float topCoef, float topPow, float botCoef, float botPow)
        {
            padTopFlex = new FlexPolynomial(topCoef, topPow);
            padBotFlex = new FlexPolynomial(botCoef, botPow);
        }

        public void setSelfHorizontalPadding(FlexPolynomial leftFlex, FlexPolynomial rightFlex)
        {
            selfPadLeftFlex = leftFlex;
            selfPadRightFlex = rightFlex;

        }

        public void setSelfHorizontalPadding(float leftCoef, float leftPow, float rightCoef, float rightPow)
        {
            selfPadLeftFlex = new FlexPolynomial(leftCoef, leftPow);
            selfPadRightFlex = new FlexPolynomial(rightCoef, rightPow);

        }

        public void setSelfVerticalPadding(FlexPolynomial topFlex, FlexPolynomial botFlex)
        {
            selfPadTopFlex = topFlex;
            selfPadBotFlex = botFlex;

        }

        public void setSelfVerticalPadding(float topCoef, float topPow, float botCoef, float botPow)
        {
            selfPadTopFlex = new FlexPolynomial(topCoef, topPow);
            selfPadBotFlex = new FlexPolynomial(botCoef, botPow);

        }

        public void setSize(Vector2 thisSize)
        {
            
            //Debug.Log(UI);
            if (dontModify)
            {
                // Debug.Log("Here");
                // Debug.Log(size);
                // Debug.Log(UI.sizeDelta);

                size = UI.sizeDelta;
            }
            else if (customDim)
            {
                if (customSize.magnitude > 0)
                {

                    //Use Custom Size
                    //Debug.Log("here");
                    size = customSize;
                }
                else
                {
                    defaultMethod(thisSize);
                }

            }
            else if (useChildMulti)
            {
                //Apply child Multi
                if (childMultiH != 0)
                {
                    Debug.Log("Hello");
                    size = new Vector2(defaultWidthCalc(thisSize.x), children.Count * childMultiH);
                }
                if (childMultiW != 0)
                {
                    Debug.Log("Hello");
                    size = new Vector2(children.Count * childMultiW, defaultHeightCalc(thisSize.y));
                }

            }
            else
            {

                defaultMethod(thisSize);
            }
            //Recalc size using new Equations and self padding

            if (layoutGroupVert)
            {
                //Calc Horizontal first
                wVal = solveW();
                hVal = solveH();
            }
            else
            {
                //Calc vertical first
                hVal = solveH();
                wVal = solveW();
            }

            if (layoutGroup)
            {
                if (layoutGroupVert)
                {
                    //Top
                    if (padTopFlex.power == 0)
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.top = (int)(padTopFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.top = (int)(padTopFlex.coefficient * hVal);
                    //Bot
                    if (padBotFlex.power == 0)
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.bottom = (int)(padBotFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.bottom = (int)(padBotFlex.coefficient * hVal);
                    //Left
                    if (padLeftFlex.power == 0)
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(padLeftFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.left= (int)(padLeftFlex.coefficient * wVal);
                    //Right
                    if (padRightFlex.power == 0)
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.right = (int)(padRightFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().padding.right = (int)(padRightFlex.coefficient * wVal);

                    //Spacing
                    if (spacingFlex.power == 0)
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().spacing = (int)((spacingFlex.coefficient)); // UI.gameObject.GetComponent<VerticalLayoutGroup>().spacing = (int)((spacingFlex.coefficient) / (children.Count - 1));  add a setting for this
                    else
                        UI.gameObject.GetComponent<VerticalLayoutGroup>().spacing = (int)((spacingFlex.coefficient * hVal) / (children.Count - 1));

                }
                else
                {
                    //Top
                    if (padTopFlex.power == 0)
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.top = (int)(padTopFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.top = (int)(padTopFlex.coefficient * hVal);
                    //Bot
                    if (padBotFlex.power == 0)
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.bottom = (int)(padBotFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.bottom = (int)(padBotFlex.coefficient * hVal);
                    //Left
                    if (padLeftFlex.power == 0)
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.left = (int)(padLeftFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.left = (int)(padLeftFlex.coefficient * wVal);
                    //Right
                    if (padRightFlex.power == 0)
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.right = (int)(padRightFlex.coefficient);
                    else
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().padding.right = (int)(padRightFlex.coefficient * wVal);

                    //Spacing
                    if (spacingFlex.power == 0)
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = (int)((spacingFlex.coefficient)); // UI.gameObject.GetComponent<VerticalLayoutGroup>().spacing = (int)((spacingFlex.coefficient) / (children.Count - 1));  add a setting for this
                    else
                        UI.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = (int)((spacingFlex.coefficient * wVal) / (children.Count - 1));

                }
            }

            //Set current object and all children
            UI.sizeDelta = size;

            //Loop through children
            for (int i = 0; i < children.Count; i++)
            {
                if (layoutGroupVert)
                {
                    if (children[i].fillParent)
                    {
                        children[i].setSize(size);
                    }
                    else if (children[i].square)
                    {

                        //Set wVal
                        // Debug.Log("Square Vert");
                        children[i].setSize(new Vector2(wVal, wVal));

                    }
                    else
                    {
                        children[i].setSize(new Vector2(wVal, hVal * children[i].flex));
                    }

                    //Debug.Log(children[i].UI);
                    // Debug.Log(children[i].size);
                }
                else
                {

                    if (children[i].fillParent)
                    {
                        children[i].setSize(size);
                    }
                    else if (children[i].square)
                    {
                        // Debug.Log("Setting child with square value");

                        // Debug.Log("Square Hor");
                        //Set hVal
                        children[i].setSize(new Vector2(hVal, hVal));
                        // }
                    }
                    else
                    {
                        children[i].setSize(new Vector2(wVal * children[i].flex, hVal));
                    }

                    // Debug.Log(children[i].UI);
                    // Debug.Log(children[i].size);
                }
            }

            //Now this should theoretically be ready to test
        }

        public float solveH()
        {
            //Vertical
            Equation eqY = new Equation();

            //Add padding flexes
            eqY.addPolynomial(padTopFlex);
            eqY.addPolynomial(padBotFlex);

            //Add spacing Flexes
            if (layoutGroupVert && layoutGroup)
            {
                eqY.addPolynomial(spacingFlex.coefficient, spacingFlex.power);
            }

            //Add children flexes
            if (layoutGroupVert)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].square)
                    {
                        //Debug.Log("Child is square");

                        // Debug.Log("Make equal to width");
                        // Debug.Log(wVal);
                        // Debug.Log(size);

                        Debug.Log("Square");
                        eqY.displayAllPoly();
                        eqY.addPolynomial(Mathf.Min(size.y, size.x), 0); //wVal
                        Debug.Log("After Add");
                        eqY.displayAllPoly();

                    }
                    else
                    {
                        eqY.addPolynomial(children[i].flex, 1);
                    }
                }
            }
            else
            {
                eqY.addPolynomial(1, 1);
            }


            return eqY.solveSingleEQ(size.y);
        }

        public float solveW()
        {
            Equation eqX = new Equation();

            //Adding padding Flexes 
            eqX.addPolynomial(padLeftFlex);
            eqX.addPolynomial(padRightFlex);


            //Add spacing flex
            if (!layoutGroupVert && layoutGroup)
            {

                if (spacingFlex.power > 0)
                {
                    spacingFlex.setVariable("x");
                }

                eqX.addPolynomial(spacingFlex.coefficient, spacingFlex.power);
            }
            //Add children
            if (!layoutGroupVert)
            {
               
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].square)
                    {
                        // Debug.Log("Child is square");
                        // Debug.Log("Make equal to height");
                        // Debug.Log(hVal);
                        //  Debug.Log(size);
                        // Debug.Log(Mathf.Min(size.y, size.x));
                        Debug.Log("Square");
                        eqX.addPolynomial(Mathf.Min(size.y, size.x), 0); //hval
                    }
                    else
                    {
                        eqX.addPolynomial(children[i].flex, 1);
                    }
                }
            }
            else
            {
                eqX.addPolynomial(1, 1);
            }

            Debug.Log("Width");
            return eqX.solveSingleEQ(size.x, name);
        }

        //Next week or if we get to use this create a system that allows you to make a blueprint or prefab type thing and then you can input a list of rectTransforms to instance which things will be affected

        public static Flex newObj(RectTransform UI, float flex)
        {
            return new Flex(UI, flex);
        }

        public void setSquare()
        {
            square = true;
        }

        public static RectTransform getChildRect(RectTransform rect, int childNum)
        {
            return rect.gameObject.transform.GetChild(childNum).GetComponent<RectTransform>();
        }

        public void setLayoutType(bool Vert)
        {
            layoutGroupVert = Vert;
        }

        public void setFillParent(bool fill)
        {
            fillParent = fill;
        }

        public void setAllPadSame(FlexPolynomial pad)
        {
            //Add a option to do it in pixels for all padding and spacing for later
            padTopFlex = pad;
            padBotFlex = pad;
            padLeftFlex = pad;
            padRightFlex = pad;
        }

        public void setAllPadSame(float padCoef, float padPow)
        {
            //Add a option to do it in pixels for all padding and spacing for later
            padTopFlex = new FlexPolynomial(padCoef, padPow);
            padBotFlex = new FlexPolynomial(padCoef, padPow);
            padLeftFlex = new FlexPolynomial(padCoef, padPow);
            padRightFlex = new FlexPolynomial(padCoef, padPow);
        }

        public void setDontModify()
        {
            dontModify = true;
        }

        public void setCustomSize(Vector2 size)
        {
            customDim = true;
            customSize = size;
        }

        public void setChildMulti(float WidthMulti, float HeightMulti)
        {
            useChildMulti = true;
            childMultiW = WidthMulti;
            childMultiH = HeightMulti;
        }


        public void defaultMethod(Vector2 thisSize)
        {
            // size = new Vector2(width.solveX(thisSize.x), height.solveX(thisSize.y));

            size = new Vector2(defaultWidthCalc(thisSize.x), defaultHeightCalc(thisSize.y));
        }

        //Default method to calculate the unit of one Flex value in Width
        public float defaultWidthCalc(float fullSize)
        {
            Equation width = new Equation();
            //Padding
            width.addPolynomial(selfPadLeftFlex);
            width.addPolynomial(selfPadRightFlex);
            //Self flex
            width.addPolynomial(1, 1);

            return width.solveSingleEQ(fullSize);
        }

        //Default method to calculate the unit of one Flex value in Height
        public float defaultHeightCalc(float fullSize)
        {
            Equation height = new Equation();
            //Padding
            height.addPolynomial(selfPadTopFlex);
            height.addPolynomial(selfPadBotFlex);
            //Self flex
            height.addPolynomial(1, 1);


            return height.solveSingleEQ(fullSize);
        }

        public void setUI (RectTransform UI)
        {
            this.UI = UI;
        }

        
        public void getChildrenFlex ()
        {
            children = new List<Flex>();
            //Check if there are any children under the parent
            if (UI.gameObject.transform.childCount > 0)
            {
                //Loop through all the children and if they have a Flex Component add it to the list of children
                foreach (Transform i in UI.gameObject.transform)
                {
                    //Check if the object has a flex component
                    if (i.gameObject.GetComponent<FlexInfo>() != null)
                    {
                        Flex childFlex = i.gameObject.GetComponent<FlexInfo>().flex;

                        //Add child to the List
                        addChild(childFlex);

                        //Get all children from the current child
                        childFlex.getChildrenFlex();

                    }
                }
            } else
            {
                return;
            }
        }

        public void setLayoutGroup (bool layout, bool vert)
        {
            layoutGroup = layout;
            layoutGroupVert = vert;
        }
        
    }
}

