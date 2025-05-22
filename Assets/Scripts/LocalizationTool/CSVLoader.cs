using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LocalizationTool
{
    public class CSVLoader
    {
        private TextAsset _csvFile;
        private char _lineSeperator = '\n';
        private char _surrunder = '"';
        private string[] _fieldSeparator = {"\",\""};

        public void LoadCsv()
        {
            _csvFile = Resources.Load<TextAsset>("Localization/Localization");
        }

        public Dictionary<string, string> GetDictionaryValues(string atributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] lines = _csvFile.text.Split(_lineSeperator);
            var attributeIndex = 1;
            string[] header =  lines[0].Split(_fieldSeparator, StringSplitOptions.None);
            
            for (int i = 0; i < header.Length; i++)
            {
                if (header[i].Contains(atributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] field = csvParser.Split(line);

                for (int j = 0; j < field.Length; j++)
                {
                    field[j] = field[j].Trim().Trim(_surrunder);
                }

                if (field.Length > attributeIndex)
                {
                    var fieldKey = field[0];
                    if (dictionary.ContainsKey(fieldKey)) continue;
                    var value = field[attributeIndex];
                    dictionary.Add(fieldKey, value);
                }
            }
            return dictionary;
        }
    }
}