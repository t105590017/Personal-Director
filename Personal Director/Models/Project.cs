﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.Models
{
    public class Project
    {
        public string Name { get; set; }

        public Project()
        {
            this.Guid = System.Guid.NewGuid();
        }

        public Project(Guid guid)
        {
            this.Guid = guid;
        }
        public Project(Project project)
        {
            this.Guid = System.Guid.NewGuid();
            this.Date = project.Date;
        }
        public DateTime Date { get; }

        public Guid Guid { get; private set; }

        public IEnumerable<Media> MediaCabinetList { get; set; }
        public IEnumerable<Media> MediaScriptList { get; set; }
    }
}
