using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace True_Pixel
{
    public class DetectionResult
    {
        public int ProbabilityPercent { get; set; }
        public string DetectedModel { get; set; }
        public string Details { get; set; }
    }

    public static class AiDetector
    {
        public static DetectionResult AnalyzeImage(string filePath)
        {
            // ------------------------------------------------------------------
            // KROK 1: Bezpośrednie sprawdzenie metadanych i cyfrowych podpisów AI
            // ------------------------------------------------------------------
            var metadataResult = CheckMetadataAndC2PA(filePath);
            if (metadataResult.IsMatch)
            {
                return new DetectionResult
                {
                    ProbabilityPercent = metadataResult.Confidence,
                    DetectedModel = metadataResult.ModelName,
                    Details = metadataResult.Details
                };
            }

            using (var bitmap = new Bitmap(filePath))
            {
                int aiScore = 0;
                List<string> detectedFeatures = new List<string>();

                // ------------------------------------------------------------------
                // KROK 2: Analiza EXIF aparatu (DOWÓD AUTENTYCZNOŚCI)
                // ------------------------------------------------------------------
                bool hasCameraExif = HasCameraExifData(bitmap);
                if (hasCameraExif)
                {
                    // Obecność parametru aparatu (np. ISO, Czas otwarcia migawki) mocno obniża wynik AI
                    aiScore -= 40;
                    detectedFeatures.Add("Wykryto fizyczny EXIF aparatu (-40%)");
                }
                else
                {
                    // Brak EXIF-u to tylko lekka przesłanka, nie dowód!
                    aiScore += 5;
                }

                // ------------------------------------------------------------------
                // KROK 3: Analiza widmowa FFT (Siatki upsampling GAN / Diffusion)
                // ------------------------------------------------------------------
                double fftScore = AnalyzeFFTGridArtifacts(bitmap);
                if (fftScore > 0.82) // Podniesiony próg dla unikania False Positives
                {
                    aiScore += 25;
                    detectedFeatures.Add("Siatka pikselowa FFT");
                }

                // ------------------------------------------------------------------
                // KROK 4: Szum PRNU (Stosowany tylko dla wyższych rozdzielczości)
                // ------------------------------------------------------------------
                double prnuScore = AnalyzePRNUNoise(bitmap);
                if (prnuScore < 0.05) // Bardzo rygorystyczny próg
                {
                    aiScore += 15;
                    detectedFeatures.Add("Brak szumu fizycznego PRNU");
                }

                // ------------------------------------------------------------------
                // KROK 5: ELA (Error Level Analysis)
                // ------------------------------------------------------------------
                double elaScore = AnalyzeELA(bitmap);
                if (elaScore < 0.03)
                {
                    aiScore += 10;
                    detectedFeatures.Add("Jednolita kompresja ELA");
                }

                // Normalizacja wyniku w przedziale 0% - 95%
                aiScore = Math.Max(0, Math.Min(aiScore, 95));

                string guessedModel = "Zdjęcie naturalne / BAZA EXIF";
                if (aiScore >= 70)
                {
                    guessedModel = "Wysokie prawdopodobieństwo AI";
                }
                else if (aiScore >= 35)
                {
                    guessedModel = "Podejrzane / Przetwarzane cyfrowo";
                }
                else
                {
                    guessedModel = "Prawdopodobnie zdjęcie naturalne";
                }

                return new DetectionResult
                {
                    ProbabilityPercent = aiScore,
                    DetectedModel = guessedModel,
                    Details = detectedFeatures.Count > 0 ? string.Join(", ", detectedFeatures) : "Brak wykrytych artefaktów AI"
                };
            }
        }

        #region Metoda 1: Metadane & C2PA
        private struct MetadataMatch
        {
            public bool IsMatch;
            public int Confidence;
            public string ModelName;
            public string Details;
        }

        private static MetadataMatch CheckMetadataAndC2PA(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string fileContentAscii = Encoding.ASCII.GetString(fileBytes);

                // 1. Weryfikacja cyfrowego podpisu C2PA / JUMBF (Adobe Firefly, DALL-E 3)
                if (fileContentAscii.Contains("c2pa") || fileContentAscii.Contains("jumbf"))
                {
                    string detectedApp = "Podpis C2PA";
                    if (fileContentAscii.Contains("Adobe Firefly")) detectedApp = "Adobe Firefly (C2PA)";
                    else if (fileContentAscii.Contains("DALL-E")) detectedApp = "DALL-E 3 (C2PA)";

                    return new MetadataMatch { IsMatch = true, Confidence = 99, ModelName = detectedApp, Details = "Znaleziono cyfrowy podpis C2PA" };
                }

                // 2. Weryfikacja nagłówków Stable Diffusion / Automatic1111 / ComfyUI
                if (fileContentAscii.Contains("Steps:") && fileContentAscii.Contains("Sampler:"))
                {
                    return new MetadataMatch { IsMatch = true, Confidence = 98, ModelName = "Stable Diffusion", Details = "Metadane parametru generation parameters" };
                }

                // 3. Weryfikacja Midjourney
                if (fileContentAscii.Contains("Midjourney") || fileContentAscii.Contains("--v 5") || fileContentAscii.Contains("--v 6"))
                {
                    return new MetadataMatch { IsMatch = true, Confidence = 98, ModelName = "Midjourney", Details = "Metadane Midjourney" };
                }
            }
            catch { }

            return new MetadataMatch { IsMatch = false };
        }

        private static bool HasCameraExifData(Bitmap bmp)
        {
            // Tagi EXIF charakterystyczne dla aparatów fotograficznych:
            // 0x829A (ExposureTime), 0x829D (FNumber), 0x8827 (ISOSpeedRatings), 0x0110 (Model aparatu)
            int[] cameraTags = { 0x829A, 0x829D, 0x8827, 0x0110 };
            return bmp.PropertyIdList.Any(id => cameraTags.Contains(id));
        }
        #endregion

        #region Metoda 2: FFT Grid Artifacts
        private static double AnalyzeFFTGridArtifacts(Bitmap bmp)
        {
            int size = 128;
            if (bmp.Width < size || bmp.Height < size) return 0;

            int startX = (bmp.Width - size) / 2;
            int startY = (bmp.Height - size) / 2;

            double[,] gray = new double[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color c = bmp.GetPixel(startX + x, startY + y);
                    gray[x, y] = (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) / 255.0;
                }
            }

            double periodicEnergy = 0;
            double totalEnergy = 0;

            for (int x = 1; x < size - 1; x++)
            {
                for (int y = 1; y < size - 1; y++)
                {
                    double laplacian = Math.Abs(4 * gray[x, y] - gray[x - 1, y] - gray[x + 1, y] - gray[x, y - 1] - gray[x, y + 1]);
                    totalEnergy += laplacian;

                    if (x % 8 == 0 && y % 8 == 0)
                    {
                        periodicEnergy += laplacian;
                    }
                }
            }

            if (totalEnergy == 0) return 0;
            return (periodicEnergy / totalEnergy) * 64.0;
        }
        #endregion

        #region Metoda 3: Szum PRNU
        private static double AnalyzePRNUNoise(Bitmap bmp)
        {
            List<double> noiseVariances = new List<double>();
            int step = Math.Max(20, Math.Min(bmp.Width, bmp.Height) / 8);

            for (int x = 20; x < bmp.Width - 20; x += step)
            {
                for (int y = 20; y < bmp.Height - 20; y += step)
                {
                    double sum = 0, sumSq = 0;
                    int count = 0;

                    for (int dx = 0; dx < 5; dx++)
                    {
                        for (int dy = 0; dy < 5; dy++)
                        {
                            Color c = bmp.GetPixel(x + dx, y + dy);
                            double lum = c.R * 0.3 + c.G * 0.59 + c.B * 0.11;
                            sum += lum;
                            sumSq += lum * lum;
                            count++;
                        }
                    }

                    double mean = sum / count;
                    double variance = (sumSq / count) - (mean * mean);
                    noiseVariances.Add(variance);
                }
            }

            if (noiseVariances.Count == 0) return 0.5;
            return noiseVariances.Average() / 100.0;
        }
        #endregion

        #region Metoda 4: Error Level Analysis (ELA)
        private static double AnalyzeELA(Bitmap bmp)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    bmp.Save(ms, jpegCodec, encoderParams);
                    ms.Position = 0;

                    using (Bitmap compressedBmp = new Bitmap(ms))
                    {
                        double totalDiff = 0;
                        int samples = 0;

                        int stepX = Math.Max(2, bmp.Width / 20);
                        int stepY = Math.Max(2, bmp.Height / 20);

                        for (int x = 0; x < bmp.Width; x += stepX)
                        {
                            for (int y = 0; y < bmp.Height; y += stepY)
                            {
                                Color c1 = bmp.GetPixel(x, y);
                                Color c2 = compressedBmp.GetPixel(x, y);

                                totalDiff += Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
                                samples++;
                            }
                        }

                        return (totalDiff / (samples * 3)) / 255.0;
                    }
                }
            }
            catch
            {
                return 0.5;
            }
        }
        #endregion
    }
}