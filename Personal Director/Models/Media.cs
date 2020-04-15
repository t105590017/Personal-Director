using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Personal_Director.Models
{
    public class Media
    {
        public Media()
        {
            this.Guid = System.Guid.NewGuid();
        }

        public Media(Guid guid)
        {
            this.Guid = guid;
        }
        public Media(Media media)
        {
            this.Guid = System.Guid.NewGuid();
            this.Thumbnail = media.Thumbnail;
            this.Describe = media.Describe;
            this.SourceGuid = media.Guid;
        }

        public Guid Guid { get; private set; }
        public Guid SourceGuid { get; private set; }

        public ImageSource Thumbnail { get; set; }

        public string Describe { get; set; }
    }
}
