using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class FileManager
{
    public static string fileName = "sav00.txt";
    private static string saveFolder = "saves";
    private static char saveToken = '='; //this divides keys from values in the file, not allowed to use
    public static string[] savings = {"Sav01.txt", "Sav02.txt", "Sav03.txt"};
    public static bool[] doesExist = new bool[3];
    private static string dirPath = null;
    public static int saveFile = 1;

    public static Dictionary<string, string> data;

    public static string getPath(int i){
        string dirPath = Directory.GetParent(Application.dataPath).FullName;
        dirPath = Path.Combine(dirPath, saveFolder);
        return dirPath + "/" + savings[i];
    }
    static FileManager(){ //static constructor, called automatically :3

        data = new Dictionary<string, string>();
        dirPath = Directory.GetParent(Application.dataPath).FullName;
        dirPath = Path.Combine(dirPath, saveFolder);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        Debug.Log($"Save Load file set to {dirPath}");


    }

    public static void SetFileName(string newFileName){
        fileName = newFileName;
    }

    public static int getSaveSlot(){
        return saveFile;
    }

    public static void setSaveSlot(int num){
        saveFile = num;
    }

    public static void LoadFromFile(){
        string filePath = Path.Combine(dirPath, fileName);
        data.Clear();

        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(saveToken);
                    if (parts.Length == 2)
                    {
                        data[parts[0].Trim()] = parts[1].Trim();
                    }
                }
                Debug.Log($"Loaded save file: {fileName}");
            }
        }
        catch
        {
            Debug.LogError("Could not load from file.");
        }
    }

    public static void SaveToFile(){
        if(getSaveSlot() > 3){
            Debug.Log("ALL SLOTS FULL!");
            return;
        }
        string filePath = Path.Combine(dirPath, fileName);

        try
        {
            using (StreamWriter wr = new StreamWriter(filePath, false))
            {
                foreach (var pair in data)
                {
                    wr.WriteLine($"{pair.Key}{saveToken}{pair.Value}");
                }
            }
            Debug.Log($"Saved to file: {fileName}");
        }
        catch
        {
            Debug.LogError("Could not save to file.");
        }
        if(saveFile > 3){
            saveFile -= 1;
        }
        doesExist[saveFile-1] = true;
        FileManager.setSaveSlot(getSaveSlot() + 1);
    }

    public static void Flush(){
        SaveToFile();
    }

    public static void SaveData(string key, string value){
        data[key] = value;
    }

    public static string GetData(string key, string defaultValue = ""){
        return data.TryGetValue(key, out string value) ? value : defaultValue;
    }

    public static void SaveInt(string key, int value){
        data[key] = value.ToString();
    }

    public static int LoadInt(string key, int defaultValue = 0){
        if (data.TryGetValue(key, out string value))
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
        }
        return defaultValue;
    }

    public static void SaveFloat(string key, float value){
        data[key] = value.ToString("F6");
    }

    public static float LoadFloat(string key, float defaultValue = 0f){
        if (data.TryGetValue(key, out string value))
        {
            if (float.TryParse(value, out float result))
            {
                return result;
            }
        }
        return defaultValue;
    }

    public static bool[] FileCheck(){
        bool[] check = {false, false, false};
        int i = 0;
        foreach(bool b in check){
            if(System.IO.File.Exists(getPath(i))){
                check[i] = true;
            }
            else{
                check[i] = false;
            }
            i++;
        }
        i = 0;
        foreach(bool b in check){
            if(!b){
               setSaveSlot(i+1);
               break;
            }
            i++;
        }
        return check;
    }

}