using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ProcessedDocument
{
    public class ProcessedDocument
    {
        public string Name { get; set; }

        public string Content { get; set; }

        private ProcessedDocument(string name, string content) 
        {
            Name = name;
            Content = content;
        }

        public ProcessedDocument Create(string name, string content)
        {
            if (string.IsNullOrEmpty(name))
            {

            }
            if (string.IsNullOrEmpty(content))
            {

            }

            return new ProcessedDocument(name, content);
        }
    }
}
