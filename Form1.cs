using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace True_Pixel
{
    public partial class Form1 : Form
    {
        // Importy funkcji Win32 API do zmiany koloru paska postępu
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int PBM_SETBARCOLOR = 0x0409;

        // Lista wspieranych rozszerzeń plików graficznych
        private readonly string[] _supportedExtensions = new[]
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".tif", ".tiff", ".gif", ".avif", ".heic"
        };

        public Form1()
        {
            InitializeComponent();

            // Ustawienie żółtego koloru paska postępu
            SetProgressBarColor(pbProgress, Color.Yellow);
        }

        // Metoda modyfikująca kolor paska postępu
        private void SetProgressBarColor(ProgressBar pBar, Color color)
        {
            SetWindowTheme(pBar.Handle, "", "");
            SendMessage(pBar.Handle, PBM_SETBARCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(color));
        }

        // Pomocnicza metoda dodająca pojedynczy plik do listy
        private void AddFileToList(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            // Przeliczenie rozmiaru na KB/MB
            string sizeText = fileInfo.Length > 1024 * 1024
                ? $"{fileInfo.Length / (1024.0 * 1024.0):F2} MB"
                : $"{fileInfo.Length / 1024.0:F1} KB";

            // Tworzenie wiersza tabeli z 5 kolumnami
            ListViewItem item = new ListViewItem(fileInfo.Name);
            item.SubItems.Add(sizeText);
            item.SubItems.Add("Nie sprawdzono"); // Status AI (index 2)
            item.SubItems.Add("-");              // Model AI (index 3)
            item.SubItems.Add("-");              // Oznaczyć jako AI (index 4)
            item.Tag = filePath;                 // Pełna ścieżka do pliku w Tagu

            lvFiles.Items.Add(item);
        }

        // 1. Dodawanie plików do listy przyciskiem
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog1.FileNames)
                {
                    AddFileToList(filePath);
                }
            }
        }

        // 2. Obsługa Drag and Drop - Najechanie plikiem na kontrolkę ListView
        private void lvFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // 3. Obsługa Drag and Drop - Upuszczenie plików na kontrolkę ListView
        private void lvFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files != null)
            {
                foreach (string filePath in files)
                {
                    string ext = Path.GetExtension(filePath).ToLower();

                    if (_supportedExtensions.Contains(ext))
                    {
                        AddFileToList(filePath);
                    }
                }
            }
        }

        // 4. Usuwanie plików z listy: jeśli są zaznaczone - usuwa zaznaczone, w przeciwnym razie czyści całą listę
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvFiles.SelectedItems)
                {
                    lvFiles.Items.Remove(item);
                }
            }
            else
            {
                lvFiles.Items.Clear();
            }

            // Czyszczenie podglądu jeśli lista jest pusta lub nic nie zaznaczono
            if (lvFiles.Items.Count == 0 || lvFiles.SelectedItems.Count == 0)
            {
                ClearPreview();
            }
        }

        // 5. Podgląd wybranego obrazu z automatyczną korektą orientacji EXIF
        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
            {
                string filePath = lvFiles.SelectedItems[0].Tag.ToString();

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        ClearPreview();
                        Image img = Image.FromStream(stream);

                        // Automatyczne obrócenie zdjęcia zgodnie z metadanymi EXIF
                        FixOrientation(img);

                        pbPreview.Image = img;
                    }
                }
                catch
                {
                    ClearPreview();
                }
            }
        }

        // Funkcja odczytująca tag EXIF 0x0112 (Orientation) i obracająca obraz
        private void FixOrientation(Image img)
        {
            if (Array.IndexOf(img.PropertyIdList, 0x0112) > -1)
            {
                var prop = img.GetPropertyItem(0x0112);
                int orientation = prop.Value[0];

                switch (orientation)
                {
                    case 3: // Obrót o 180 stopni
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 6: // Obrót o 90 stopni w prawo
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 8: // Obrót o 90 stopni w lewo
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                img.RemovePropertyItem(0x0112);
            }
        }

        private void ClearPreview()
        {
            if (pbPreview.Image != null)
            {
                pbPreview.Image.Dispose();
                pbPreview.Image = null;
            }
        }

        // 6. Główna funkcja - sprawdzanie plików (Asynchroniczna z paskiem postępu)
        private async void btnCheck_Click(object sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                MessageBox.Show("Dodaj najpierw pliki do analizy.", "Brak plików", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Blokada przycisków na czas analizy
            btnCheck.Enabled = false;
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;

            // Inicjalizacja paska postępu
            pbProgress.Minimum = 0;
            pbProgress.Maximum = lvFiles.Items.Count;
            pbProgress.Value = 0;
            pbProgress.Visible = true;

            try
            {
                int processedCount = 0;

                foreach (ListViewItem item in lvFiles.Items)
                {
                    string filePath = item.Tag.ToString();

                    if (File.Exists(filePath))
                    {
                        // Wywołanie analizy w osobnym wątku (Task.Run) - zapobiega ścinaniu UI
                        DetectionResult result = await Task.Run(() => AiDetector.AnalyzeImage(filePath));

                        // Wyłączenie automatycznego dziedziczenia stylów dla subitemów
                        item.UseItemStyleForSubItems = false;

                        // Ustawienie czarnego koloru dla pierwszych kolumn
                        item.SubItems[0].ForeColor = Color.Black;
                        item.SubItems[1].ForeColor = Color.Black;

                        // Wpisanie wyniku AI i modelu
                        item.SubItems[2].Text = $"{result.ProbabilityPercent}%";
                        item.SubItems[3].Text = result.DetectedModel;

                        // Wyróżnienie kolorystyczne procentowego wyniku AI
                        if (result.ProbabilityPercent >= 70)
                        {
                            item.SubItems[2].ForeColor = Color.Red;
                        }
                        else if (result.ProbabilityPercent >= 35)
                        {
                            item.SubItems[2].ForeColor = Color.Orange;
                        }
                        else
                        {
                            item.SubItems[2].ForeColor = Color.DarkGreen;
                        }

                        item.SubItems[3].ForeColor = Color.Black;

                        // Weryfikacja progu 55% dla nowej kolumny "Oznaczyć jako AI"
                        if (result.ProbabilityPercent >= 55)
                        {
                            item.SubItems[4].Text = "Tak";
                            item.SubItems[4].ForeColor = Color.Red;
                        }
                        else
                        {
                            item.SubItems[4].Text = "Nie";
                            item.SubItems[4].ForeColor = Color.DarkGreen;
                        }
                    }
                    else
                    {
                        item.SubItems[2].Text = "Błąd";
                        item.SubItems[3].Text = "Plik nie istnieje";
                        item.SubItems[4].Text = "-";
                    }

                    // Aktualizacja paska postępu po przetworzeniu każdego pliku
                    processedCount++;
                    pbProgress.Value = processedCount;
                }

                MessageBox.Show("Analiza plików została zakończona!", "Gotowe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas analizy: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Przywrócenie dostępności przycisków i ukrycie paska postępu
                btnCheck.Enabled = true;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                pbProgress.Visible = false;
            }
        }

        // 7. Okno informacyjne
        private void btnInfo_Click(object sender, EventArgs e)
        {
            string instructionText =
                "=== INSTRUKCJA OBSŁUGI PROGRAMU TRUE PIXEL ===\n\n" +
                "1. DODAWANIE PLIKÓW:\n" +
                "   • Kliknij 'Dodaj pliki' lub przeciągnij pliki na listę (JPG, PNG, BMP, WEBP, TIF, GIF, AVIF, HEIC).\n\n" +
                "2. ANALIZA AI:\n" +
                "   • Kliknij 'Sprawdź czy wygenerowane przez AI'. \n\nProgram przeanalizuje metadane C2PA/EXIF, siatki FFT, szum PRNU oraz kompresję ELA.\n\n" +
                "3. OZNACZENIA WYNIKÓW:\n" +
                "   • Oznaczenie 'Tak' - zgodnie z rozporządzeniem Parlamentu Europejskiego i Rady (UE) 2024/1689 zdjęcie należy oznaczyć jako AI\n" +
                "   • Oznaczenie 'Nie' - zgodnie z rozporządzeniem Parlamentu Europejskiego i Rady (UE) 2024/1689 zdjęcia nie należy oznaczać jako AI\n\n" +
                "4. PODGLĄD:\n" +
                "   • Kliknięcie pliku na liście wyświetla jego podgląd z automatycznym obróceniem EXIF.";

            MessageBox.Show(instructionText, "Instrukcja obsługi - True Pixel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}