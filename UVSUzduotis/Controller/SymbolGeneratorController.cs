﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVSUzduotis.Controller
{
    public class SymbolGeneratorController
    {
        private static readonly Random random = new Random();

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string GenerateSymbols(int minLenght, int maxLenght)
        {
            if(minLenght > maxLenght || minLenght < 0)
            {
                throw new ArgumentException("minLength must be less than or equal to maxLenght and non-negative");
            }

            int lenght = random.Next(minLenght, maxLenght +1);
            
            //A task that repeats the output based on the lenght input and randomises the output with Random().
            return new string(Enumerable.Repeat(chars, lenght).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
