using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

namespace Stom
//=========================================================================
//=========Class optimize string
//=========================================================================
{
    public class StringFast
    {
        //Immutable string. Generated at last moment, only if needed
        private string stringGenerated = "";
        //<summary>Is m_stringGenerated is up to date 
        private bool isStringGenerated = false;

        //Working mutable string
        private char[] chars = null;
        private int charsCount = 0;
        private int charsCapacity = 0;

        //Temporary string used for the Replace method
        private List<char> m_replacement = null;

        private object m_valueControl = null;
        private int m_valueControlInt = int.MinValue;

        /// <summary>
        /// Initial string
        /// </summary>
        /// <param name="initialCapacity"></param>
        public StringFast(int initialCapacity = 32) { chars = new char[charsCapacity = initialCapacity]; }

        /// <summary>
        /// Check empty string
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() { return (isStringGenerated ? (stringGenerated == null) : (charsCount == 0)); }

        /// <summary>
        /// Return string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Regenerate the immutable string if needed
            if (!isStringGenerated)
            {
                stringGenerated = new string(chars, 0, charsCount);
                isStringGenerated = true;
            }
            return stringGenerated;
        }

        ///<summary>
        /// Return true if the valueControl has changed (and update it)
        /// </summary>
        public bool IsModified(int newControlValue)
        {
            bool changed = (newControlValue != m_valueControlInt);
            if (changed)
                m_valueControlInt = newControlValue;
            return changed;
        }

        ///<summary>
        /// Return true if the valueControl has changed (and update it)
        /// </summary>
        public bool IsModified(object newControlValue)
        {
            bool changed = !(newControlValue.Equals(m_valueControl));
            if (changed)
                m_valueControl = newControlValue;
            return changed;
        }

        public void Set(string str)
        {
            Clear();
            Append(str);
        }
        ///<summary>
        /// Caution, allocate some memory
        /// </summary>
        public void Set(object str)
        {
            Set(str.ToString());
        }
        public void Set<T1, T2>(T1 str1, T2 str2)
        {
            Clear();
            Append(str1); Append(str2);
        }
        public void Set<T1, T2, T3>(T1 str1, T2 str2, T3 str3)
        {
            Clear();
            Append(str1); Append(str2); Append(str3);
        }
        public void Set<T1, T2, T3, T4>(T1 str1, T2 str2, T3 str3, T4 str4)
        {
            Clear();
            Append(str1); Append(str2); Append(str3); Append(str4);
        }
        ///
        /// <summary>Allocate a little memory (20 byte)
        /// </summary>
        public void Set(params object[] str)
        {
            Clear();
            for (int i = 0; i < str.Length; i++)
                Append(str[i]);
        }


        public StringFast Clear()
        {
            charsCount = 0;
            isStringGenerated = false;
            return this;
        }

        /// <summary>
        /// Append string method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public StringFast Append(string value)
        {
            ReallocateIFN(value.Length);
            int n = value.Length;
            for (int i = 0; i < n; i++)
                chars[charsCount + i] = value[i];
            charsCount += n;
            isStringGenerated = false;
            return this;
        }
        ///<summary>
        /// Append an object.ToString(), allocate some memory
        /// </summary>
        public StringFast Append(object value)
        {
            Append(value.ToString());
            return this;
        }

        ///<summary>
        /// Append an int without memory allocation
        /// </summary>
        public StringFast Append(int value)
        {
            // Allocate enough memory to handle any int number
            ReallocateIFN(16);

            // Handle the negative case
            if (value < 0)
            {
                value = -value;
                chars[charsCount++] = '-';
            }

            // Copy the digits in reverse order
            int nbChars = 0;
            do
            {
                chars[charsCount++] = (char)('0' + value % 10);
                value /= 10;
                nbChars++;
            } while (value != 0);

            // Reverse the result
            for (int i = nbChars / 2 - 1; i >= 0; i--)
            {
                char c = chars[charsCount - i - 1];
                chars[charsCount - i - 1] = chars[charsCount - nbChars + i];
                chars[charsCount - nbChars + i] = c;
            }
            isStringGenerated = false;
            return this;
        }

        ///<summary>
        /// Append a float without memory allocation
        /// </summary>
        public StringFast Append(float value)
        {
            // Allocate enough memory to handle any float number
            ReallocateIFN(16);

            // Handle the negative case
            if (value < 0)
            {
                value = -value;
                chars[charsCount++] = '-';
            }

            // Transform the float into an int and get the number of floating digits
            int nbFloatDigits = 0;
            while (Mathf.Abs(value - Mathf.Round(value)) > 0.00001f)
            {
                value *= 10;
                nbFloatDigits++;
            }
            int valueInt = Mathf.RoundToInt(value);

            // Copy the digits in reverse order
            int nbChars = 0;
            do
            {
                chars[charsCount++] = (char)('0' + valueInt % 10);
                valueInt /= 10;
                nbChars++;
                // Add the point
                if (nbFloatDigits == nbChars)
                {
                    chars[charsCount++] = '.';
                    nbChars++;
                }
            } while (valueInt != 0 || nbChars <= nbFloatDigits + 1);

            // Reverse the result
            for (int i = nbChars / 2 - 1; i >= 0; i--)
            {
                char c = chars[charsCount - i - 1];
                chars[charsCount - i - 1] = chars[charsCount - nbChars + i];
                chars[charsCount - nbChars + i] = c;
            }
            isStringGenerated = false;
            return this;
        }

        ///<summary>
        /// Replace all occurences of a string by another one
        /// </summary>
        public StringFast Replace(string oldStr, string newStr)
        {
            if (charsCount == 0)
                return this;

            if (m_replacement == null)
                m_replacement = new List<char>();

            // Create the new string into m_replacement
            for (int i = 0; i < charsCount; i++)
            {
                bool isToReplace = false;
                // If first character found, check for the rest of the string to replace
                if (chars[i] == oldStr[0])
                {
                    int k = 1;
                    while (k < oldStr.Length && chars[i + k] == oldStr[k])
                        k++;
                    isToReplace = (k >= oldStr.Length);
                }

                // Do the replacement
                if (isToReplace)
                {
                    i += oldStr.Length - 1;
                    if (newStr != null)
                        for (int k = 0; k < newStr.Length; k++)
                            m_replacement.Add(newStr[k]);
                }
                else
                    // No replacement, copy the old character
                    m_replacement.Add(chars[i]);
            }

            // Copy back the new string into m_chars
            ReallocateIFN(m_replacement.Count - charsCount);
            for (int k = 0; k < m_replacement.Count; k++)
                chars[k] = m_replacement[k];
            charsCount = m_replacement.Count;
            m_replacement.Clear();
            isStringGenerated = false;
            return this;
        }

        private void ReallocateIFN(int nbCharsToAdd)
        {
            if (charsCount + nbCharsToAdd > charsCapacity)
            {
                charsCapacity = Math.Max(charsCapacity + nbCharsToAdd, charsCapacity * 2);
                char[] newChars = new char[charsCapacity];
                chars.CopyTo(newChars, 0);
                chars = newChars;
            }
        }
    }
}