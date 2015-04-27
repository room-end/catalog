namespace catalog.viewer.Model.Photography
{
    using System;

    public sealed class sd_Film : Film
    {
        public override FilmType type
        {
            get { return FilmType.sd; }
        }

        public override int type2
        {
            get { return 4; }
        }
    }
}