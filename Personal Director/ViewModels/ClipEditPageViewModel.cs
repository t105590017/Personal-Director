using Personal_Director.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.ViewModels
{
    public class ClipEditPageViewModel
    {

        public ClipEditPageViewModel(StoryBoard storyBoard)
        {
            this.StoryBoard = storyBoard;
        }

        public StoryBoard StoryBoard { get; set; }
    }
}
