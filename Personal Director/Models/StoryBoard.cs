using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.Models
{
    public class StoryBoard
    {
        public Media MediaSource { get; set; }

        private List<Effect> _effects;

        public StoryBoard(Media media)
        {
            this.Guid = Guid.NewGuid();
            this.MediaSource = media;
            this._effects = new List<Effect>();
        }

        public Guid Guid { get; private set; }

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
