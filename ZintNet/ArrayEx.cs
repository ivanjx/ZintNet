/* ArrayExtensions.cs - Addsome functionality to the Array Class */

/*
    Copyright (C) 2013-2016 Milton Neal <milton200954@gmail.com>

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:

    1. Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer.
    2. Redistributions in binary form must reproduce the above copyright
       notice, this list of conditions and the following disclaimer in the
       documentation and/or other materials provided with the distribution.
    3. Neither the name of the project nor the names of its contributors
       may be used to endorse or promote products derived from this software
       without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
    FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
    OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
    HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
    OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
    SUCH DAMAGE.
 */
using System;
using System.Text;

namespace ArrayExt
{
    /// <summary>
    /// Extended methods for arrays.
    /// </summary>
    internal class ArrayEx
    {
        /// <summary>
        /// Converts a character array to uppercase.
        /// </summary>
        /// <param name="data">input array</param>
        /// <returns>converted array</returns>
        public static char[] ToUpper(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 96 && data[i] < 123)
                    data[i] -= (char)32;
            }

            return data;
        }

        // Insert integer into the array at position.
        public static int[] Insert(int[] data, int position, int value)
        {
            int length = data.Length + 1;

            if (position > length)
                throw new ArgumentOutOfRangeException("position");

            Array.Resize(ref data, length);
            for (int i = length - 1; i > position; i--)
                data[i] = data[i - 1];

            data[position] = value;
            return data;
        }

        /// <summary>
        /// Inserts a byte into the array at a specified position.
        /// </summary>
        /// <param name="data">array to insert the data byte</param>
        /// <param name="position">position to insert the byte</param>
        /// <param name="value">byte value to insert</param>
        /// <returns>the new array</returns>
        public static byte[] Insert(byte[] data, int position, byte value)
        {
            int length = data.Length + 1;

            if (position > length)
                throw new ArgumentOutOfRangeException("position");

            System.Array.Resize(ref data, length);
            for (int i = length - 1; i > position; i--)
                data[i] = data[i - 1];

            data[position] = value;
            return data;
        }

        public static byte[] Insert(byte[] data, int position, byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                data = Insert(data, position, bytes[i]);
                position++;
            }

            return data;
        }

        // Insert character into the array at position.
        public static char[] Insert(char[] data, int position, char value)
        {
            int length = data.Length + 1;

            if (position > length)
                throw new ArgumentOutOfRangeException("position");

            Array.Resize(ref data, length);
            for (int i = length - 1; i > position; i--)
                data[i] = data[i - 1];

            data[position] = value;
            return data;
        }

        // Insert characters into the array at position.
        public static char[] Insert(char[] data, int position, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                data = Insert(data, position, chars[i]);
                position++;
            }

            return data;
        }

        public static char[] Insert(char[] data, int position, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                data = Insert(data, position, str[i]);
                position++;
            }

            return data;
        }

        // Removes the character at position from the array.
        public static char[] Remove(char[] data, int position)
        {
            int length = data.Length;

            if (position > length)
                throw new ArgumentOutOfRangeException("position");

            for (int i = position; i < length - 1; i++)
                data[i] = data[i + 1];

            Array.Resize(ref data, length - 1);
            return data;
        }

        public static byte[] Remove(byte[] data, int position)
        {
            int length = data.Length;

            if (position > length)
                throw new ArgumentOutOfRangeException("position");

            for (int i = position; i < length - 1; i++)
                data[i] = data[i + 1];

            System.Array.Resize(ref data, length - 1);
            return data;
        }
    }
}
