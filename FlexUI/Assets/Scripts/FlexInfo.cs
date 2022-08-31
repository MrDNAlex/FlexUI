using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FlexUI
{


    public class FlexInfo : MonoBehaviour
    {
        public Flex flex;
       
        private void Awake()
        {
            //Set the RectTransform
            flex.setUI(gameObject.GetComponent<RectTransform>());

            //Check if the object has a LayoutGroup (Horizontal or Vertical)
            if (gameObject.GetComponent<HorizontalLayoutGroup>() != null || gameObject.GetComponent<VerticalLayoutGroup>() != null)
            {
                //Check which of the layout groups are attached 
                if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
                {
                    //Set the layoutGroup to vertical
                    flex.setLayoutGroup(true, true);
                } else
                {
                    //Set the LayoutGroup to Horizontal
                    flex.setLayoutGroup(true, false);
                }
            } else
            {
                //Set the Layout group to false
                flex.setLayoutGroup(false, false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            flex.getChildrenFlex();

            if (flex.size.magnitude > 0 )
            {
                
                flex.setSize(flex.size);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }



}

