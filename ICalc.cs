using System;
using System.IO;
using System.Net;

namespace ICalendarToPng {

    public class ICalc {

        private readonly string _iCalcUrl;


        public ICalc(string iCalcUrl) {
            _iCalcUrl = iCalcUrl;
        }


        public void DownloadFile() {
            using (var client = new WebClient()) {
                Console.WriteLine("Downloading file from {0}", _iCalcUrl);
                client.DownloadFile(_iCalcUrl, @"..\..\res\ithsSchema.ics");
            }
        }

        public string ReadFile() {
            string file;

            using (var sr = new StreamReader(@"..\..\res\ithsSchema.ics")) {
                file = sr.ReadToEnd();
            }

            return file;
        }

        public string ReadFile(string fileDir) {
            string file;

            using (var sr = new StreamReader(fileDir)) {
                file = sr.ReadToEnd();
            }

            return file;
        }

    }

}