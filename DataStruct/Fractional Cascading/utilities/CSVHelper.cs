using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace Fractional_Cascading {
    public class Record {
        public int n { get; set; }
        public int k { get; set; }
        public int x { get; set; }
        public float kBinarySearchesTime { get; set; }
        public float fractionalCascadingTime { get; set; }
        public float Ratio { get; set; }
        public Record(int nVal, int kVal, int xVal,
                      float trivialTimeVal, float FCTimeVal, float ratio) {
            n = nVal;
            k = kVal;
            x = xVal;
            Ratio = ratio;
            kBinarySearchesTime = trivialTimeVal;
            fractionalCascadingTime = FCTimeVal;
        }
    }

    public class CSVHelper {
        /**
        Functionality to output search stats to a CSV file  */

        private string filePath;
        
        public void writeCSV(List<Record> recs) {
            using (StreamWriter writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                csv.WriteRecords(recs);
            }
        }

        public CSVHelper(string filePathString) {
            filePath = filePathString;
        }
    }
}