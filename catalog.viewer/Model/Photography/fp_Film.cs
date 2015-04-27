namespace catalog.viewer.Model.Photography
{
    using System;

    public sealed class fp_Film : Film
    {
        public override FilmType type
        {
            get { return FilmType.fp; }
        }

        public override int type2
        {
            get { return 2; }
        }
    }
}