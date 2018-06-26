using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileModel
{
    public abstract class Export
    {        
        public abstract String exportPath { get; set; }
        public abstract void export();
        public abstract void addRow(String[] row);
        public abstract void openStream();

        
    }
}
