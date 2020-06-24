using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Production.MediaProcess;
using Production.Model;

namespace Personal_Director.Models
{
    public class StoryBoard
    {
        public Media MediaSource { get; set; }

        private List<IEffect> _effects;

        public StoryBoard(Media media)
        {
            this.Guid = Guid.NewGuid();
            this.MediaSource = media;
            VideoHandler.SetSource(this.Guid, media.SourcePath);
            this._effects = new List<IEffect>();
        }

        public StoryBoard(Guid guid, Media media, List<IEffect> effects)
        {
            this.Guid = guid;
            this.MediaSource = media;
            this._effects = effects;
        }

        public Guid Guid { get; private set; }

        public void AddEffect(IEffect effect)
        {
            this._effects.Add(effect);
        }

        public List<IEffect> GetAllEffects()
        {
            return this._effects;
        }

        public void RemoveEffect(IEffect effect)
        {
            this._effects.Remove(effect);
        }
    }
}
