// See https://aka.ms/new-console-template for more information
using FalloutSolver.Solver;

namespace OCRTest
{
    internal class Program
    {
        static byte[] ReadImage(string path)
        {
            using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(file);

            var bytes = new List<byte>();
            var buffer = new byte[2048];
            int count;

            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
                bytes.AddRange(buffer.Take(count));
            }
            return bytes.ToArray();
        }

        static void Main(string[] args)
        {
            using var capture = new OcrCapture();

            var test = (string path) =>
            {
                var image = ReadImage(path);
                var (confidence, text) = capture.GetText(image);
                Console.WriteLine("{0}: {1}", path, confidence);
                Console.WriteLine(text);
                Console.WriteLine("==========================================================");
            };

            //test("sample_capture.jpg");
            //test("sample_cropped.png");
            test("sample_inverted_skewed.png");

        }
    }
}

