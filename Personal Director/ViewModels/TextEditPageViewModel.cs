using Personal_Director.Models;
using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Production.Model;

namespace Personal_Director.ViewModels
{
    public class TextEditPageViewModel
    {
        public StoryBoard StoryBoard { get; set; }

        public TextEditPageViewModel(StoryBoard storyBoard)
        {
            this.StoryBoard = storyBoard;
        }

        public string GetProcessedMediaPath(string text, Production.Enum.VideoPosition position, Color color, string font, int size)
        {
            // TODO: 字型與字體大小還未寫
            IEffect effect = new TextEffect(text, position, color, Fontsize:size, Fontfile:font);
            effect.SetDataSource(this.StoryBoard.Guid, this.StoryBoard.MediaSource.SourcePath);
            effect.Excute();
            this.StoryBoard.AddEffect(effect);
            return effect.OutputPath;
        }
    }
}
