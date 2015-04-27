namespace catalog.viewer.Model.Photography
{
    using System;

    public sealed class fn_Film : Film
    {
        public override FilmType type
        {
            get { return FilmType.fn; }
        }

        public override int type2
        {
            get { return 3; }
        }
    }
}