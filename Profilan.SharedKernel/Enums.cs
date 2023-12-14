namespace Profilan.SharedKernel
{
    public enum TrackingState
    {
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }

    public enum ContentType
    {
        DOC,
        DOCX,
        XLS,
        XLSX,
        VSD,
        VDX,
        PPT,
        PPTX,
        XDW,
        PDF,
        XPS,
        JPEG,
        JPG,
        BMP,
        PNG,
        TIF,
        TIFF,
        GIF,
        SVG,
        TXT,
        RTF,
        XML,
        CSV,
        UNKNOWN = -1
    }

    public enum ConversionType
    {
        Doc2Pdf,
        Pdf2Img,
        Img2Img,
        Ppt2Vid
    }

    public enum Unit
    {
        Hours = 1,
        Minutes = 2,
        Seconds = 3,
        Days = 4,
        Months = 5,
        Years = 6
    }
}
