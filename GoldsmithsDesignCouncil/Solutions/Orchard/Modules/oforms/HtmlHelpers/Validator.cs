using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;


namespace oforms.HtmlHelpers
{
    public class Validator {
        public Dictionary<string, List<String>> _validationDictionary; 

        public  Validator() {
            _validationDictionary = new Dictionary<string, List<String>>();
        }

        public Validator ValidationRuleCollector(string attribute, string rule)
        {
            if (attribute== null) {
                return this;
            }
            string[] ruleCollector = attribute.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ruleCollector.Count(); i++)
            {
                ruleCollector[i] = ruleCollector[i].Trim();
                if (!_validationDictionary.ContainsKey(ruleCollector[i]))
                {
                    _validationDictionary.Add(ruleCollector[i],new List<string>());
                }
                if (ruleCollector[i].Length > 0)
                {
                    _validationDictionary[ruleCollector[i]].Add(rule);
                }
            }
            return this;
        }

        public string RenderValidator() {

            List<String> collectRules = new List<String>();
            List<String> collectFields = new List<String>();

            foreach (var pair in _validationDictionary) {
                collectRules.Clear();
                var renderedRule = "\n"+pair.Key+": {\n";
                foreach (var rules in pair.Value) {
                    collectRules.Add("\t"+rules + " : true");
                }
                renderedRule += string.Join(",\n", collectRules)+"\n}";
                collectFields.Add(renderedRule);
            }
            return string.Join(",\n", collectFields);

        }
    }
}