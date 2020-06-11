using Personal_Director.Models;
using Production.MediaProcess;
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

        public string GetClippedMediaPath(TimeSpan startTime, TimeSpan endTime) 
        {
            VideoHandlerObject videoHandlerObject =  VideoHandler.SetSource(this.StoryBoard.Guid, this.StoryBoard.MediaSource.SourcePath)
                            .CutVideo(startTime, endTime);

            return videoHandlerObject.OutputPath;
        }
    }
}
