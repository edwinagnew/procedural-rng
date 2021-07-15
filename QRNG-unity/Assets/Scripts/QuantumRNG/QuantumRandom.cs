using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

public class QuantumRandom {

    public static float Range(float minInclusive, float maxInclusive) {
        int minNumBits = minNumOfBits((int)maxInclusive);
        string filePath = "Assets/Bits/rng_data.txt";
        int result = ReadOutput(filePath, minNumBits, (int)minInclusive, (int)maxInclusive);
        return (float)result;
    }

    public static float Range(float maxInclusive){
        int minNumBits = minNumOfBits((int)maxInclusive);
        string filePath = "Assets/Bits/rng_data.txt";
        int result = ReadOutput(filePath, minNumBits, 0, (int)maxInclusive);
        return (float)result;
    }

    public static int Range(int minInclusive, int maxInclusive) {
        int minNumBits = minNumOfBits(maxInclusive);
        string filePath = "Assets/Bits/rng_data.txt";
        int result = ReadOutput(filePath, minNumBits, minInclusive, maxInclusive);
        return result;
    }

    public static int Range(int maxInclusive){
        int minNumBits = minNumOfBits(maxInclusive);
        string filePath = "Assets/Bits/rng_data.txt";
        int result = ReadOutput(filePath, minNumBits, 0, maxInclusive);
        return result;
    }

    private static int minNumOfBits(int maxNumInclusive) {
        int bits = 1;
        while (maxNumInclusive > 1) {
            maxNumInclusive /= 2;
            bits++;
        }
        return bits;
    }

    private static int ReadOutput(string filePath, int bitsPerOutput, int minAcceptedValue, int maxAcceptedValue) {
        Debug.Log(filePath);
        if (!File.Exists(filePath)) {
            Debug.Log("File does not exist");
            return -1;
        } else if (new FileInfo(filePath).Length == 0) {
            Debug.Log("File has no data");
            return -1;
        }
        
        long fileLength = new FileInfo(filePath).Length;

        char[] buffer = new char[bitsPerOutput];
        char[] charsRead = new char[fileLength - bitsPerOutput];

        int numCharsRead = 0;
        
        string value = String.Empty;

        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite)) {
            using (StreamReader reader = new StreamReader(stream)) {
                while (numCharsRead < fileLength) {
                    int currCharsRead = reader.Read(buffer, 0, buffer.Length);

                    for (int i = 0; i < buffer.Length; i++) {
                        Debug.Log(buffer[i]);
                    }

                    value = String.Join(String.Empty, buffer);
                    int result = Convert.ToInt32(value, 2);
                    if (result >= minAcceptedValue && result <= maxAcceptedValue) {
                        reader.Read(charsRead, numCharsRead, ((int)fileLength - (numCharsRead + buffer.Length)));
                        using (StreamWriter writer = new StreamWriter(stream)) { 
                            stream.SetLength(0);
                            writer.Write(charsRead, 0, charsRead.Length);
                        }
                        return result;
                    }
                    Array.Copy(buffer, 0, charsRead, numCharsRead, buffer.Length);
                    numCharsRead += buffer.Length;
                }
            }
        }

        return -1;
    }
}
