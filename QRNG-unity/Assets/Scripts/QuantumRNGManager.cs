using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

using QuantumRNG;


public class QuantumRNGManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    const string pythonEXE = @"/.q/python.exe";

#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        const string pythonEXE = @"/.q/bin/python";

#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
        const string pythonEXE = @"/.q/bin/python";

#else
        //const string pythonEXE = @"/.q/python.exe";
        //notimplemented yet

#endif

    public WritePythonFile writer;
    public int numQubits;
    [SerializeField]
    private string token;
    public bool TranspileCircuit;

    public string dataFile = "rng_data.txt";

    PythonJob myJob;

    private bool wrote_results;




    // void prepareAndStartJob() {

    //     string pythonPath = Application.streamingAssetsPath + pythonEXE;
    //     string filePath = Application.streamingAssetsPath + "/PythonScript/" + "RNGjob.py";

    //     myJob = new PythonJob();
    //     myJob.PythonPath = pythonPath;
    //     myJob.FilePath = filePath;
    //     myJob.FinishedCallback += finishedJobCallback;

    //     myJob.Start();
    // }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("writing job file");


        writer = new WritePythonFile(numQubits, token, TranspileCircuit);
        writer.writeJobFile();


        string pythonPath = Application.streamingAssetsPath + pythonEXE;


        myJob = new PythonJob();
        myJob.PythonPath = pythonPath;
        myJob.FilePath = writer.pythonFilePath;
        myJob.FinishedCallback += finishedJobCallback;

        myJob.Start();
    }

    // Update is called once per frame
    void Update() {
        if (myJob != null) {
            if (myJob.Update()) {
                // Alternative to the OnFinished callback
                finishJob();
                myJob = null;
            } else {
                doWaiting();
            }
        }
    }

    //gets called each frame while the job is running. This normally is not needed but you coud do things here.
    void doWaiting() {

    }

    void finishedJobCallback(string output) {
        Debug.Log("Finished Job Callback was called with output:" + output);
        
    }

    void finishedWritingCallback(string output){
        Debug.Log("actually made it!");

        //string data_file_path = Application.dataPath + "/Bits/rng_data.txt";

    }

    void finishJob() {


        if(!wrote_results){

            string[] splits = myJob.Output.Split(',');

            Debug.Log("write output: " + splits[0] + splits[1]);

            string backend = splits[0];
            string job_id = splits[1].Substring(0, splits[1].Length - 1);
            
            Debug.Log("JOB ID LENGTH: " + job_id.Length);

            Debug.Log("Job finished with output: " + backend + " " + job_id);

            // string save_path = Application.dataPath + "/Bits/" + backend + "_" + job_id + ".txt";
            string[] savePathArr = {Application.dataPath, "/Bits/", job_id, ".txt"};
            string save_path = string.Join("", savePathArr);

            Debug.Log("SAVE PATH IS : " + save_path);
            string pythonPath = Application.streamingAssetsPath + pythonEXE;

            string data_file_path = Application.dataPath + "/Bits/" + dataFile;
    

            writer.writeReadFile(backend, job_id, token, save_path, data_file_path);

            myJob = new PythonJob();
            myJob.PythonPath = pythonPath;
            myJob.FilePath = writer.pythonFilePath;
            myJob.FinishedCallback += finishedWritingCallback;

    

            myJob.Start();

            //wrote_results = true;

        }
        else{
            Debug.Log("We have the results!");
        }

    }
}
