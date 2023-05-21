using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace FalloutSolver.Solver
{
    public class OcrCapture : IDisposable
    {
        private bool disposedValue = false;
        private readonly TesseractEngine _Engine;

        public OcrCapture()
        {
            _Engine = new TesseractEngine(".", "eng");
        }

        public (double, string) GetText(byte[] image)
        {
            using var img = Pix.LoadFromMemory(image);
            using var page = _Engine.Process(img);
            var confidence = page.GetMeanConfidence();
            var text = page.GetText();
            return (confidence, text);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~OcrCapture()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
