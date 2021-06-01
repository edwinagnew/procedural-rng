using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuantumRNG {
    public class WritePythonFile
    {

        public int numQubits;
        public string token;
        public string pythonFilePath;
        public bool transpile;

        public WritePythonFile(int numberOfQubits, string accountToken, bool transpileCircuit) {
            numQubits = numberOfQubits;
            token = accountToken;
            pythonFilePath = Application.streamingAssetsPath + "/PythonScript/" + "RNGjob.py";
            transpile = transpileCircuit;
        }


        public string buildString() {
            string imports =        "from qiskit import IBMQ, QuantumCircuit, assemble, execute\nfrom qiskit.providers.ibmq import least_busy\nfrom math import pi";
            string providerInfo =   "IBMQ.enable_account('" + token + "')\nprovider = IBMQ.get_provider('ibm-q')";
            string backendInfo =    "num_qubits = " + numQubits + "\navailable_backends = provider.backends(filters=lambda device: device.configuration().n_qubits >= num_qubits and not device.configuration().simulator)\nbackend = least_busy(available_backends)";
            string circuitInitInfo = "qc = QuantumCircuit(num_qubits)";
            string circuitOpInfo =  "thetas = getThetas(backend, num_qubits)\nfor i in range(num_qubits):\n\tqc.ry(thetas[i], i)\nqc.measure_all()";
            string jobInfo;
            if (transpile){
                jobInfo = "job = execute(qc, backend, shots=8192,memory=True)\njob_id = job.job_id()";
                }
            else{
                jobInfo =       "qobj = assemble(qc)\njob=backend.run(qobj)\njob_id = job.job_id()";
            }

            string printInfo = "print(str(backend) + ',' + job_id)";

            string thetaFunctionInfo = "def getThetas(backend, n):\n\treturn [pi/2]*n"; //for now - will update soon

            return imports + "\n" + thetaFunctionInfo + "\n" + providerInfo + "\n" + backendInfo + "\n" + circuitInitInfo + "\n" + circuitOpInfo  + "\n" + jobInfo + "\n" + printInfo;
        }

        public void writeJobFile() {
            string pythonCode = buildString();
            //Debug.Log("TEST");

            //Debug.Log("Writing python script to " + pythonFilePath);

            //Debug.Log(numQubits);
            //Debug.Log(token);

            File.WriteAllText(pythonFilePath, pythonCode);

        }

        public void writeReadFile(string backend, string job_id, string token, string results_path, string data_path){

            UnityEngine.Debug.Log("reading" + results_path);

            string imports = "from qiskit import IBMQ\nfrom zipfile import ZipFile";
            string providerInfo =   "IBMQ.enable_account('" + token + "')\nprovider = IBMQ.get_provider('ibm-q')";
            string jobInfo = "backend = provider.get_backend('" + backend + "')\njob = provider.backend.retrieve_job('" + job_id + "')";
            string waitInfo = "job.wait_for_final_state()";
            string saveInfo = "results = job.result().get_memory()\nfile = open('" + results_path + "', 'w')\nfor r in results: file.write(r + ',')\nfile.close()";
            string zipInfo = "zip_name = file.name.rstrip('.txt') + '.zip'\nwith ZipFile(zip_name,'w') as zip: zip.write(file.name)";
            string extraWriteInfo = "f = open('" + data_path + "', 'a')\nfor r in results: f.write(r)\nf.close()";

            string printInfo = "print('done', job.status())\nprint('saved to', zip_name, 'and " +  data_path + "')";

            string code = imports + "\n" + providerInfo + "\n" + jobInfo + "\n" + waitInfo + "\n" + saveInfo + "\n" + zipInfo + "\n"  + extraWriteInfo + "\n" + printInfo;

            File.WriteAllText(pythonFilePath, code);
        }

    }
}
