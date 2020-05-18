using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace ObjectComparer
{
    public static class Comparer
    {
        /// <summary>
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns><c>true</c> if all property values
        ///           are equal, otherwise <c>false</c>.</returns>
        public static bool AreSimilar<T>(T first, T second)
        {

            bool result;
            if (first != null && second != null)
            {
                result = true; // assume by default they are equal
                Type objectType;
                objectType = first.GetType();
                Comparator comparator = ComparatorFactory.GetComparator(objectType);
                if (!comparator.AreEntitiesEqual(first, second))
                {
                    return false;
                }
            }
            else
                result = object.Equals(first, second);

            return result;
        }
    }


    /// <summary>
    /// The 'Comparator' abstract class
    /// </summary>
    public abstract class Comparator
    {
        public abstract bool AreEntitiesEqual<T>(T first, T second);
    }


    public class PrimitiveComparator : Comparator
    {
        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns><c>true</c> if both values match,
        ///              otherwise <c>false</c>.
        ///</returns>
        public override bool AreEntitiesEqual<T>(T first, T second)
        {
            bool result;
            IComparable selfValueComparer;
            selfValueComparer = first as IComparable;

            if (first == null && second != null || first != null && second == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(second) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(first, second))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }
    }

    public class ListComparator : Comparator
    {
        /// <summary>
        /// Compares two list's values and returns if they are the same.
        /// </summary>
        /// <param name="first">The first list's values to compare.</param>
        /// <param name="second">The second list's values to compare.</param>
        /// <returns><c>true</c> if both values match,
        ///              otherwise <c>false</c>.
        ///</returns>
        public override bool AreEntitiesEqual<T>(T valueA, T valueB)
        {
            if (valueA == null && valueB == null)
            {
                return object.Equals(valueA, valueB);
            }
            

            System.Collections.Generic.IEnumerable<object> collectionItems1;
            System.Collections.Generic.IEnumerable<object> collectionItems2;
            int collectionItemsCount1;
            int collectionItemsCount2;

            // null check
            if (valueA == null && valueB != null || valueA != null && valueB == null)
            {
                return false;
            }
            else if (valueA != null && valueB != null)
            {
                collectionItems1 = ((IEnumerable)valueA).Cast<object>();
                collectionItems2 = ((IEnumerable)valueB).Cast<object>();
                collectionItemsCount1 = collectionItems1.Count();
                collectionItemsCount2 = collectionItems2.Count();

                // check the counts to ensure they match
                if (collectionItemsCount1 != collectionItemsCount2)
                {
                    return false;
                }
                // and if they do, compare each item...
                // this assumes both collections have the same order
                else
                {
                    for (int i = 0; i < collectionItemsCount1; i++)
                    {
                        object collectionItem1;
                        object collectionItem2;
                        Type collectionItemType;

                        collectionItem1 = collectionItems1.ElementAt(i);
                        collectionItem2 = collectionItems2.ElementAt(i);
                        collectionItemType = collectionItem1.GetType();
                        Comparator comparator = ComparatorFactory.GetComparator(collectionItemType);

                        if (!comparator.AreEntitiesEqual(collectionItem1, collectionItem2))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    public class ArrayComparator : Comparator
    {
        /// <summary>
        /// Compares two array's values and returns if they are the same.
        /// </summary>
        /// <param name="first">The first array's values to compare.</param>
        /// <param name="second">The second array's values to compare.</param>
        /// <returns><c>true</c> if both values match,
        ///              otherwise <c>false</c>.
        ///</returns>
        public override bool AreEntitiesEqual<T>(T valueA, T valueB)
        {
            if (valueA == null && valueB == null)
            {
                return object.Equals(valueA,valueB);
            }

            Type objectType;
            objectType = valueA.GetType();

            var collectionsItems1 = valueA as Array;
            var collectionsItems2 = valueB as Array;

            var collectionItemsCount1 = collectionsItems1.Length;
            var collectionItemsCount2 = collectionsItems2.Length;
            if (collectionItemsCount1 != collectionItemsCount2)
            {
                return false;
            }
            Comparator comparator = ComparatorFactory.GetComparator(objectType.GetElementType());


            //Exrta Condition :: compare array regardless of element's order; 
            //Sorting array if it is of premetive type
            if (typeof(IComparable).IsAssignableFrom(objectType.GetElementType()) || objectType.GetElementType().IsPrimitive)
            {
                Array.Sort(collectionsItems1);
                Array.Sort(collectionsItems2);
            }


            for (int i = 0; i < collectionsItems1.Length; i++)
            {
                if (!comparator.AreEntitiesEqual(collectionsItems1.GetValue(i), collectionsItems2.GetValue(i)))
                {
                    return false;
                }
            }


            return true;
        }
    }

    public class ClassComparator : Comparator
    {
        /// <summary>
        /// Compares class objects of same type and returns if their properties value are the same.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns><c>true</c> if both values match,
        ///              otherwise <c>false</c>.
        ///</returns>
        public override bool AreEntitiesEqual<T>(T first, T second)
        {
            if (first == null && second == null)
            {
                return object.Equals(first, second);
            }
            Type objectType;
            objectType = first.GetType();

            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead))
            {
                object valueA;
                object valueB;

                valueA = propertyInfo.GetValue(first, null);
                valueB = propertyInfo.GetValue(second, null);
                if (valueA != null && valueB != null)
                {
                    Comparator comparator = ComparatorFactory.GetComparator(propertyInfo.PropertyType);
                    if (!comparator.AreEntitiesEqual(propertyInfo.GetValue(first, null), propertyInfo.GetValue(second, null)))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!object.Equals(valueA, valueB))
                    {
                        return false;
                    }
                }

            }
            return true;
        }
    }


    /// <summary>
    /// Helps to find appropriate comparator factory.
    /// </summary>
    public static class ComparatorFactory
    {

        /// <summary>
        /// Determines which comparator should be return depending on given property type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///    appropriate <c>Comparator</c> will be return
        /// </returns>
        public static Comparator GetComparator(Type propertyType)
        {
            if (typeof(IComparable).IsAssignableFrom(propertyType) || propertyType.IsPrimitive)
            {
                return new PrimitiveComparator();
            }
            else if (propertyType.IsArray)
            {
                return new ArrayComparator();
            }
            else if (!propertyType.IsArray && propertyType.GetInterfaces().Any(s => s.Name.Equals("IEnumerable")))
            {
                return new ListComparator();
            }
            else if (propertyType.IsClass || propertyType.IsValueType) //Here value type is to compare Structs like KeyValuePair<K,V> , DateTime
            {
                return new ClassComparator();
            }
            return null;
        }
    }
}

