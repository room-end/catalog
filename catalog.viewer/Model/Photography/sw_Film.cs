namespace catalog.viewer.Model.Photography
{
    using System;

    public sealed class sw_Film : Film 
    {
        public override FilmType type
        {
            get { return FilmType.sw; }
        }

        public override int type2
        {
            get { return 1; }
        }
    }
}