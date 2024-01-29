using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CannonFightBase
{
    public static class StringFormat
    {

        public static string SetStringIconFormat(this string str)
        {
            //Debug.Log(str);
            string pattern = @"(?<=\[)\w*(?=])\w*";

            Regex rg = new Regex(pattern);
            MatchCollection matchedAuthors = rg.Matches(str);

            int minus = 0;
            int plus = 0;
            string addedString = "naberrrrrr ";
            
            //Debug.LogFormat("add: " + addedString.Length);

            for (int count = 0; count < matchedAuthors.Count; count++)
            {
                Debug.Log("BULUNDU: " + matchedAuthors[count].Index);
                str = str.Remove(matchedAuthors[count].Index - minus + plus, matchedAuthors[count].Length);

                if(count == matchedAuthors.Count - 1)
                    str = str.Remove(matchedAuthors[count].Index - minus - 1 + plus, 2);//[] leri siliyoruz
                else
                    str = str.Remove(matchedAuthors[count].Index - minus - 1 + plus, 3);//[] leri siliyoruz

                str = str.Insert(matchedAuthors[count].Index - minus - 1 + plus, addedString);

                plus += addedString.Length;
                minus += matchedAuthors[count].Length + 3;
            }



            return str;
        }

    }

}
