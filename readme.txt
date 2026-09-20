========================================================================
                      TRUE PIXEL v1.0 - README
========================================================================
========================================================================


[WERSJA POLSKA]

1. O PROGRAMIE TRUE PIXEL
-------------------------
True Pixel to otwartoźródłowa aplikacja okienkowa dla systemu Windows stworzona w języku C# (.NET / WinForms). Program służy do analizy i wykrywania obrazów wygenerowanych lub zmodyfikowanych przez sztuczną inteligencję (AI). Aplikacja wykonuje wielowarstwową analizę, obejmującą weryfikację metadanych C2PA/EXIF, analizę siatek FFT, badanie szumu PRNU oraz kompresję ELA.

2. GŁÓWNE FUNKCJE
-----------------
- Wielowarstwowa detekcja AI: Określa procentowe prawdopodobieństwo oraz identyfikuje wykryty model AI.
- Zgodność z Aktem o AI (UE): Wskazuje, czy obraz wymaga oznaczenia jako wygenerowany przez AI zgodnie z rozporządzeniem UE 2024/1689.
- Obsługa Drag and Drop: Przeciąganie plików graficznych bezpośrednio na listę w aplikacji.
- Automatyczna orientacja obrazu: Automatyczny obrót podglądu zdjęcia na podstawie metadanych EXIF.
- Szeroka obsługa formatów: JPG, JPEG, PNG, BMP, WEBP, TIF, TIFF, GIF, AVIF, HEIC.
- Otwarty kod źródłowy: Dystrybuowany na elastycznej licencji MIT.

3. OBSŁUGIWANE FORMATY PLIKÓW
-----------------------------
- JPEG / JPG (.jpg, .jpeg)
- PNG (.png)
- BMP (.bmp)
- WebP (.webp)
- TIFF / TIF (.tif, .tiff)
- GIF (.gif)
- AVIF (.avif)
- HEIC (.heic)

4. INSTRUKCJA OBSŁUGI
---------------------
1. Dodawanie plików: Kliknij przycisk "Dodaj pliki" lub przeciągnij pliki graficzne bezpośrednio na listę.
2. Analiza: Kliknij przycisk "Sprawdź czy wygenerowane przez AI", aby rozpocząć proces weryfikacji.
3. Interpretacja wyników:
   - "AI": Pokazuje procentowy wynik prawdopodobieństwa oraz model AI.
   - "Oznaczyć jako AI": Wynik "Tak" lub "Nie" wyznaczony w oparciu o próg prawny wynikający z rozporządzenia (UE) 2024/1689.
4. Podgląd: Kliknięcie dowolnej pozycji na liście wyświetla jej podgląd graficzny.

5. AUTOR I LICENCJA
-------------------
Autor: Jakub Wojciechowski
Firma: CLICK4PROFIT LTD (Numer spółki: 16943299)
Adres: 71-75 Shelton Street, Covent Garden, Londyn, WC2H 9JQ, Wielka Brytania

Oprogramowanie objęte licencją MIT. Szczegółowe informacje znajdują się w pliku LICENSE.txt.
========================================================================

[ENGLISH VERSION]

1. ABOUT TRUE PIXEL
-------------------
True Pixel is an open-source Windows desktop application built with C# (.NET / WinForms).
It is designed to detect whether images have been generated or modified by Artificial Intelligence (AI) models. The application performs multi-layered forensic checks including C2PA/EXIF metadata verification, FFT grid analysis, PRNU noise evaluation, and Error Level Analysis (ELA).

2. KEY FEATURES
---------------
- Multi-Layer AI Detection: Evaluates probability scores and identifies underlying AI generator models.
- EU AI Act Compliance Indicator: Highlights whether an image must be marked as AI under EU Regulation 2024/1689.
- Drag & Drop Support: Drag image files directly into the application window.
- Automatic Image Orientation: Automatically corrects preview rotation using EXIF metadata tags.
- Wide Format Support: JPG, JPEG, PNG, BMP, WEBP, TIF, TIFF, GIF, AVIF, HEIC.
- Open Source & Lightweight: Distributed under the permissive MIT License.

3. SUPPORTED FILE FORMATS
-------------------------
- JPEG / JPG (.jpg, .jpeg)
- PNG (.png)
- BMP (.bmp)
- WebP (.webp)
- TIFF / TIF (.tif, .tiff)
- GIF (.gif)
- AVIF (.avif)
- HEIC (.heic)

4. HOW TO USE
-------------
1. Adding Files: Click "Dodaj pliki" (Add Files) or drag and drop image files onto the main list.
2. Running Analysis: Click "Sprawdź czy wygenerowane przez AI" (Check AI Generation) to start processing.
3. Reviewing Results:
   - "AI Probability": Displays the detection percentage and detected model.
   - "Mark as AI": Indicates "Tak" (Yes) or "Nie" (No) based on the threshold mandated by Regulation (EU) 2024/1689.
4. Preview: Click any file in the list to display its image preview.

5. AUTHOR & LICENSE
-------------------
Author: Jakub Wojciechowski
Company: CLICK4PROFIT LTD (Company No. 16943299)
Address: 71-75 Shelton Street, Covent Garden, London, WC2H 9JQ, United Kingdom

Licensed under the MIT License. See LICENSE.txt for full terms.


