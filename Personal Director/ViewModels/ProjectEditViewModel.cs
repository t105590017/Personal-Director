using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Director.Models;

namespace Personal_Director.ViewModels
{
    public class ProjectEditViewModel
    {
        private Model _model;
        ProjectEditViewModel(Model model) 
        {
            this._model = model;
        }
    }
}
