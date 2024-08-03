using System;
using LorcanaSpellbook.Enums;

namespace LorcanaSpellbook.Utils
{
    public static class GeneralUtils
    {
        /// <summary>
        /// Convert from string to any enum. Throwing errors if string does not match.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringToParse"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T ConvertStringToEnum<T>(string stringToParse) where T : struct, IConvertible
        {
            Type typeToConvert = typeof(T);
            if (!typeToConvert.IsEnum)
            {
                throw new ArgumentException("This is not an enum. Why have you done this? Type: " + typeof(T));
            }

            //Get enum names
            if (Enum.TryParse<T>(stringToParse, out T result))
            {
                return result;
            }

            throw new ArgumentException("This is not a value in Enum of type" + typeof(T) + " with value: " + stringToParse);
        }
    }

}