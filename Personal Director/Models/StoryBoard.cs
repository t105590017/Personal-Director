using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.Models
{
    public class StoryBoard
    {
        public Media MediaSource { get;}

        private List<Effect> _effects;

        public StoryBoard(Media media)
        {
            this.MediaSource = media;
            this._effects = new List<Effect>();
        }

        public void AddEffect(Effect effect)
        {
            this._effects.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            this._effects.Remove(effect);
        }
    }
}
