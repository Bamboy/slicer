using UnityEngine;
using System.Collections.Generic;

//SCRIPT TAKEN FROM: http://wiki.unity3d.com/index.php/ExtRandom

public class ExtRandom<T>
{
	//This method sets a random seed for the RNG using 2 convoluted formulas. 
	//Using this method at any time other than during downtime is not recommended. 
	//This method will never see use 99.9% of the time, but is available in cases where a truly random seed is required.
	public static void RandomizeSeed()
    {
        Random.seed = System.Math.Abs(((int)(System.DateTime.Now.Ticks % 2147483648l) - (int)(Time.realtimeSinceStartup + 2000f)) / ((int)System.DateTime.Now.Day - (int)System.DateTime.Now.DayOfWeek * System.DateTime.Now.DayOfYear));
        Random.seed = System.Math.Abs((int)((Random.value * (float)System.DateTime.Now.Ticks * (float)Random.Range(0, 2)) + (Random.value * Time.realtimeSinceStartup * Random.Range(1f, 3f))) + 1);
    }
 	
	//This method returns either true or false with an equal chance.
    public static bool SplitChance()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }
 	
	//This method returns either true or false with the chance of the former derived from the parameters passed to the method.
	// ExtRandom<int>.Chance(1, 10);
	//The above code has a 10% chance of returning true.
    public static bool Chance(int nProbabilityFactor, int nProbabilitySpace)
    {
        return Random.Range(0, nProbabilitySpace) < nProbabilityFactor ? true : false;
    }
 	
	//This method returns a random element chosen from an array of elements.
    public static T Choice(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
    public static T Choice(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
 
	//This method returns a random element chosen from an array of elements based on the respective weights of the elements.
	//For most reliable behaviour, the sum of nWeights should equal 100
    public static T WeightedChoice(T[] array, int[] nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < array.Length; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < array.Length; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return array[nChoiceIndex];
    }
    public static T WeightedChoice(List<T> list, int[] nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < list.Count; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < list.Count; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return list[nChoiceIndex];
    }
 
	//This method rearranges the elements of a list randomly and returns the rearranged list.
    public static T[] Shuffle(T[] array)
    {
        T[] shuffledArray = new T[array.Length];
        List<int> elementIndices = new List<int>(0);
        for(int i = 0; i < array.Length; i++)
        {
            elementIndices.Add(i);
        }
        int nArrayIndex;
        for(int i = 0; i < array.Length; i++)
        {
            nArrayIndex = elementIndices[Random.Range(0, elementIndices.Count)];
            shuffledArray[i] = array[nArrayIndex];
            elementIndices.Remove(nArrayIndex);
        }
 
        return shuffledArray;
    }
    public static List<T> Shuffle(List<T> list)
    {
        List<T> shuffledList = new List<T>(0);
        int nListCount = list.Count;
        int nElementIndex;
        for(int i = 0; i < nListCount; i++)
        {
            nElementIndex = Random.Range(0, list.Count);
            shuffledList.Add(list[nElementIndex]);
            list.RemoveAt(nElementIndex);
        }
 
        return shuffledList;
    }
}