using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Recipe
{
    public string name;
    public List<string> ingridients;
    public int IngridientCount()
    {
        return ingridients.Count;
    }
    public List<string> GetIngridients()
    {
        return  ingridients;
    }
}


[System.Serializable]
public class RecipeList
{
    public List<Recipe> list;
    public int RecipeCount()
    {
        return list.Count;
    }
}

public class Utility
{
    public RecipeList LoadJsonData(string path)
    {
        string jsonString = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<RecipeList>(jsonString);
    }
    
    public bool ListComparer(List<string> list1 , List <string> list2)
    {
               
        if (list1.Count != list2.Count)
            return false;
        List<string> temp1 = new List<string>(list1);
        List<string> temp2 = new List<string>(list2);
        temp1.Sort();
        temp2.Sort();

        for (int i = 0; i< temp1.Count; i++)
        {
            if (temp1[i] != temp2[i])
            {
                return false;
            }
        }

        return true;
    }
   
}
