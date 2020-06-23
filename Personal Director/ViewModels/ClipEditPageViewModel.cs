using Personal_Director.Models;
using Production.MediaProcess;
using Production.Model;
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

        public string GetProcessedMediaPath(TimeSpan startTime, TimeSpan endTime) 
        {
            IEffect effect = new CutEffect(startTime, endTime);
            effect.SetDataSource(this.StoryBoard.Guid, this.StoryBoard.MediaSource.SourcePath);
            effect.Excute();
            this.StoryBoard.AddEffect(effect);
            return effect.OutputPath;
        }
    }
}
