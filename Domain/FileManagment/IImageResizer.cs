using System;
using System.IO;

namespace FileManagment
{
    public interface IImageResizer
    {
        FileInfo ResizeImageByLengthOfLongestSide(FileInfo imageToResizeUri);
    }
}
