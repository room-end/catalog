namespace catalog.viewer.Model.Photography
{
    using System;

    [Flags]
    public enum FilmScanStatus
    {
        not_scanned = 0,
        incomplete = 1,
        needs_rescan = 2,
        scanned_4800 = 256,
        scanned_2800 = 128,
    }
}