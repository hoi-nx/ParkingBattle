using IvyMoon;
using UnityEngine;
using System.Collections;
/*
*see Pickup.cs , it will add to the coins count based on its value of Coins
*/

namespace IvyMoon //this creates a namespace for all of the IvyMoon scripts so they dont interfere with yours
{

    public class currency : MonoBehaviour
    {
        public int gold;
        public int silver;
        public int copper;
        public string amount;
        public int coins;
        public int Gold { get { return coins / 10000; } } //could use Silver/100
        public int Silver { get { return coins / 100; } }
        public int Copper { get { return coins; } }
        int g;
        int s;
        //	int c;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (coins != 0)
            {
                gold = coins / 10000;
                g = (int)gold;
                silver = (coins - (g * 10000)) / 100;
                s = (int)silver;
                copper = coins - (g * 10000) - (s * 100);
                //c = (int) copper;  
            }
            else
            {
                g = 0;
                s = 0;
                //c=0;
            }
            amount = "Gold:" + gold + " Silver:" + silver + " Copper:" + copper;
        }
    }
}