﻿using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DicomGrep.Services
{
    public class DictionaryService
    {
        public const string DICOM_DIC_FILE = "DICOM Dictionary.xml";
        public const string PRIVA_DIC_FILE = "Private Dictionary.xml";

        public DictionaryService()
        {
            DicomDictionary.EnsureDefaultDictionariesLoaded(true);
        }

        public void ReadAndAppendCustomDictionaries(string dicomDictionaryFilePath = DICOM_DIC_FILE, string privateDictionaryFilePath = PRIVA_DIC_FILE)
        {
            // append private tags
            if (File.Exists(privateDictionaryFilePath))
            {
                DicomDictionary.Default.Load(privateDictionaryFilePath, DicomDictionaryFormat.XML);
            }

            // append common tags
            ReadDictionary(out DicomDictionary dicomDictionary, dicomDictionaryFilePath);
            foreach (var entry in dicomDictionary)
            {
                DicomDictionary.Default.Add(entry);
            }
        }

        private bool ReadDictionary(out DicomDictionary dictionary, string filePath)
        {
            dictionary = new DicomDictionary();

            if (File.Exists(filePath))
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    DicomDictionaryReader reader = new DicomDictionaryReader(dictionary, DicomDictionaryFormat.XML, stream);
                    reader.Process();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
