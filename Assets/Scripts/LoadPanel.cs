using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class LoadPanel : MonoBehaviour
{
    public Button[] saveFiles;
    public string[] saveNames = {"Sav01.txt", "Sav02.txt", "Sav03.txt"};
    

    public int[] levelData = {0,0,0};

    // Start is called before the first frame update
    void Start()
    {

        FileManager.doesExist = FileManager.FileCheck();
        Debug.Log("This time SaveFile is: "+FileManager.saveFile.ToString());
        int i = 0;
        foreach(bool b in FileManager.doesExist){
            if(!b){
                i++;
                continue;          
            }
            else{
                FileManager.setSaveSlot(i + 1);
                i++;
            }

        }
        i = 0;
        foreach(string s in saveNames){
            FileManager.SetFileName(s);
            FileManager.LoadFromFile();
            FileManager.SaveData("FileName", s);
            TextMeshProUGUI tmpText = saveFiles[i].GetComponentInChildren<TextMeshProUGUI>();

            if (FileManager.doesExist[i]){
            // Set the text
            tmpText.text = $"File 0{i+1}\nLevel 0{FileManager.GetData("CurrentLevel")} - {FileManager.LoadFloat("TimeRemaining")}";
            FileManager.doesExist[i] = true;
            DeathLoop.openSlots[i] = 1;
            i++;
            }
            else{

                        tmpText = saveFiles[i].GetComponentInChildren<TextMeshProUGUI>();
                        tmpText.text = $"File 0{i+1}\nLevel XX - XX:XX";
                        FileManager.doesExist[i] = false;
                        DeathLoop.openSlots[i] = 0;
                        levelData[i] = 0;
                    
                }
            }
        }


    

    public void loadFile(int slot){

        if(FileManager.doesExist[slot - 1]){
            string level = "Level " + levelData[slot - 1].ToString();
            DeathLoop.SetCameFromMenu();  // Signal that we’re coming from Menu
            SceneManager.LoadScene(level);
        }
    
    }

    public void deleteFile(int slot){
        if(FileManager.doesExist[slot - 1]){
            FileManager.data.Clear();
            TextMeshProUGUI tmpText = saveFiles[slot-1].GetComponentInChildren<TextMeshProUGUI>();
            tmpText.text = $"File 0{slot}\nLevel XX - XX:XX";
            FileManager.doesExist[slot-1] = false;
            DeathLoop.openSlots[slot-1] = 0;
            levelData[slot-1] = 0;
            Debug.Log(FileManager.getPath(slot - 1).ToString());
            if(System.IO.File.Exists(FileManager.getPath(slot - 1))){
                System.IO.File.Delete(FileManager.getPath(slot - 1));
            }
        }
        else{
            TextMeshProUGUI tmpText = saveFiles[slot-1].GetComponentInChildren<TextMeshProUGUI>();
            tmpText.text = $"File 0{slot}\nLevel XX - XX:XX";
            FileManager.doesExist[slot-1] = false;
            DeathLoop.openSlots[slot-1] = 0;
            levelData[slot-1] = 0;
        }
        if(System.IO.File.Exists(FileManager.getPath(slot - 1))){
            System.IO.File.Delete(saveNames[slot-1]);
        }
        FileManager.saveFile--;
        if(FileManager.saveFile == 0){
            FileManager.saveFile++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
