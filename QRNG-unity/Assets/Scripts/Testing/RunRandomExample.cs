using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuantumRNG;

using System;
using System.IO;
using System.Text;
using System.IO.Compression;

using Random=UnityEngine.Random;


public class RunRandomExample : MonoBehaviour
{
    [SerializeField]
    public static Text unityResults;
    [SerializeField]
    public static Text quantumResults;

    // Start is called before the first frame update
    void Start()
    {
        unityResults = GameObject.Find("UnityText").GetComponent<Text>();
        quantumResults = GameObject.Find("QuantumText").GetComponent<Text>();
    
        Debug.Log(unityResults);
        Debug.Log(quantumResults);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunCoinExample() {
        // Flip a coin 8192 times
        Dictionary<string, int> unityCoinResults = new Dictionary<string, int>();
        Dictionary<string, int> quantumCoinResults = new Dictionary<string, int>();

        string[] unityArray = new string[8192];
        string[] qArray = new string[8192];

        unityCoinResults.Add("Heads", 0);
        unityCoinResults.Add("Tails", 0);
        quantumCoinResults.Add("Heads", 0);
        quantumCoinResults.Add("Tails", 0);

        for (int i = 0; i < 8192; i++) {
            int val = Random.Range(0, 2);
            unityArray[i] = val.ToString();//.Replace("\n", "");

            int qval = QuantumRandom.Range(0, 1);
            qArray[i] = qval.ToString();//.Replace("\n", "");

            if (val == 0) {
                unityCoinResults["Heads"]++;
            } else {
                unityCoinResults["Tails"]++;
            }

            if (qval == 0) {
                quantumCoinResults["Heads"]++;
            } else {
                quantumCoinResults["Tails"]++;
            }
        }

        string displayUnityResults = "Heads: " + unityCoinResults["Heads"] + "\nTails: " + unityCoinResults["Tails"]; 
        string displayQuantumResults = "Heads: " + quantumCoinResults["Heads"] + "\nTails: " + quantumCoinResults["Tails"];

        unityResults.text = displayUnityResults;
        quantumResults.text = displayQuantumResults;

        //write unity array and qarray to text files
        string bpath = Application.dataPath + "/Bits/SizeCheck/";

        File.WriteAllLines(bpath + "unityRand/unityOutput.txt", unityArray, Encoding.UTF8);
        File.WriteAllLines(bpath + "qRand/qOutput.txt", qArray, Encoding.UTF8);

        //zip files
        string uzip_path = bpath + "unityOutput.zip";
        if(!File.Exists(uzip_path)){
            ZipFile.CreateFromDirectory(bpath + "unityRand", uzip_path);

            // using (FileStream zipToOpen = new FileStream(bpath + "unityOutput.txt", FileMode.Open))
            // {
            //     using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            //     {
            //         ZipArchiveEntry readmeEntry = archive.CreateEntry("Readme.txt");
            //         using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
            //         {
            //                 writer.WriteLine("Information about this package.");
            //                 writer.WriteLine("========================");
            //         }
            //     }
            // }


            ZipFile.CreateFromDirectory(bpath + "qRand", bpath + "qOutput.zip");
        }

         
 
    

    }

    public void RunDiceExample() {
        // Roll a die 8192 times
        Dictionary<int, int> unityDiceResults = new Dictionary<int, int>();
        Dictionary<int, int> quantumDiceResults = new Dictionary<int, int>();

        unityDiceResults.Add(1, 0);
        unityDiceResults.Add(2, 0);
        unityDiceResults.Add(3, 0);
        unityDiceResults.Add(4, 0);
        unityDiceResults.Add(5, 0);
        unityDiceResults.Add(6, 0);

        quantumDiceResults.Add(1, 0);
        quantumDiceResults.Add(2, 0);
        quantumDiceResults.Add(3, 0);
        quantumDiceResults.Add(4, 0);
        quantumDiceResults.Add(5, 0);
        quantumDiceResults.Add(6, 0);


        for (int i = 0; i < 500; i++) {
            int val = Random.Range(1, 7);
            int qval = QuantumRandom.Range(1, 6);
            
            unityDiceResults[val]++;
            quantumDiceResults[qval]++;
        }

        string displayUnityResults = "1: " + unityDiceResults[1] + "\n2: " + unityDiceResults[2] + "\n3: " + unityDiceResults[3] + "\n4: " + unityDiceResults[4] + "\n5: " + unityDiceResults[5] + "\n6: " + unityDiceResults[6]; 
        string displayQuantumResults = "1: " + quantumDiceResults[1] + "\n2: " + quantumDiceResults[2] + "\n3: " + quantumDiceResults[3] + "\n4: " + quantumDiceResults[4] + "\n5: " + quantumDiceResults[5] + "\n6: " + quantumDiceResults[6];

        unityResults.text = displayUnityResults;
        quantumResults.text = displayQuantumResults;
    }

}
