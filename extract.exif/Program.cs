using System;
namespace extract.exif
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BitMiracle.LibTiff.Classic;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            string filename = args[0];
            using (Tiff image = Tiff.Open(filename, "r"))
            {
                if (image == null)
                {
                    Console.Error.WriteLine("Could not open incoming image");
                    return;
                }

                short numberOfDirectories = image.NumberOfDirectories();
                for (short d = 0; d < numberOfDirectories; ++d)
                {
                    if (d != 0)
                        Console.Out.WriteLine("---------------------------------");

                    image.SetDirectory((short)d);

                    Console.Out.WriteLine("Image {0}, page {1} has following tags set:\n", filename, d);
                    for (ushort t = ushort.MinValue; t < ushort.MaxValue; ++t)
                    {
                        TiffTag tag = (TiffTag)t;
                        FieldValue[] value = image.GetField(tag);
                        if (value != null)
                        {
                            for (int j = 0; j < value.Length; j++)
                            {
                                Console.Out.WriteLine("{0}", tag.ToString());
                                Console.Out.WriteLine("{0} : {1}", value[j].Value.GetType().ToString(), value[j].ToString());
                            }

                            Console.Out.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
