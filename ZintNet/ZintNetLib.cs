/*
    ZintNetLib - a C# port of libzint.
    Copyright (C) 2013-2020 Milton Neal <milton200954@gmail.com>
    Acknowledgments to Robin Stuart and other Zint Authors and Contributors.
 
    libzint - the open source barcode library
    Copyright (C) 2008-2020 Robin Stuart <rstuart114@gmail.com>

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:

    1. Redistributions of source code must retain the above copyright 
       notice, this list of conditions and the following disclaimer.  
    2. Redistributions in binary form must reproduce the above copyright
       notice, this list of conditions and the following disclaimer in the
       documentation and/or other materials provided with the distribution.  
    3. Neither the name of the project nor the names of its contributors
       may be used to endorse or promote products derived from this software
       without specific prior written permission. 

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
    FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
    OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
    HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
    OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
    SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SkiaSharp;
using ZintNet.Encoders;

namespace ZintNet
{
    /// <summary>
    /// ZintNet DLL Libaray.
    /// </summary>
    public class ZintNetLib : IDisposable
    {
        #region Property Fields

        SKFont mFont;	    // Base font to render text.
        float mBarcodeHeight;			// Barcode height in scale units (not including the text).
        EncodingMode mEncodingMode;     // Special message formating Standard, GS1 or HIBC.
        string mSupplementMessage;		// EAN/UPC suppliment data;
        bool mTextVisible;				// Barcode text visible if true.
        TextPosition mTextPosition;	    // Placement of the barcode text, above or below barcode;
        SKColor mTextColor;				// Color to render the barcode text.
        SKColor mBarcodeColor;			// Color to render the barcode.
        bool mOptionalCheckDigit;		// Determines if the option check digit is generated.
        bool mShowCheckDigit;			// Determines if the check digit(s) are show in the barcode text.
        int mNumberOfCheckDigits;   	            // The number of check digits used for the symbology;
        AusPostEncoding mAusPostEncoding;	        // Customer Information Field encoding for Australia Post barcode.
        MSICheckDigitType mMSICheckDigitType;	    // Type for the MSI Plessey check digit.
        I2of5CheckDigitType mI2of5CheckDigitType;   // Type for I20f5 check digit.
        float mMultiplier;				            // Barcode module width multiplier.
        float mXDimension;				            // Barcode's element X dimension value in mm.
        int mRotation;                              // Roation angle.
        float mTextMargin;				            // Space between the barcode and human readable text in mm;
        ITF14BearerStyle mITF14BearerStyle;	        // Determines bearer bar style (I2of5 only).
        TextAlignment mTextAlignment;	            // Determines how the barcode's text is displayed.
        int mChannelCodeLevel;
        int mCodablockFRows;
        int mCodablockFColumns;
        int mDotCodeColumns;
        QRCodeEccLevel mQRCodeEccLevel;
        int mECIMode;
        int mQRCodeVersion;
        DataMatrixSize mDataMatrixSize;             // Datamatrix size option.
        bool mDataMatrixSquare;                     // Datamatrix forces a square symbol.
        bool mDataMatrixRectExtn;                   // Datamatrix enable extended rectangle mode.
        int mGridMatrixVersion;
        int mGridMatrixEccLevel;
        int mAztecSize;
        int mAztecEccLevel;
        int mHanXinVersion;
        int mHanXinEccLevel;
        int mUltracodeCompression;      // UltraCode compression;
        int mUltracodeEccLevel;         // UltraCode error correction level.
        int mCodeOneSize;
        int mPDF417Columns;             // PDF417 number of columns, 0 = Auto.
        int mPDF417ErrorLevel;          // PDF417 error correction, -1 = Auto.
        int mPDF417RowHeight;           // PDF417 element height ratio, default = x2 elements;
        int mDatabarExpandedSegments;   // Segments in the Databar Expanded Stacked symbol.
        string mCompositeMessage;       // Composite symbol message.
        CompositeMode mCompositeMode;   // Composite symbol mode(CCA, CCB or CCC).
        MaxicodeMode mMaxicodeMode;     // Maxicode encoding mode.

        #endregion

        #region General Fields
        // Barcode Formating Fields
        Collection<SymbolData> encodedData; // Holds the encoded data for each character in the barcode data.
        Symbology symbolId;			    // The current barcode symbology.
        string barcodeMessage;			// Barcode message.
        bool isValidFlag;			    // Flag to indicate if encoding was successful and a valid barcode is generated.
        float bearerWidth;				// Width of the ITF-14 bearer(mm).
        bool isEanUpc;                  // True if the symbol is an EAN/UPC or ISBN sysmbol.
        int linearShiftCount;           // The number of element the Ean/Upc with composite is shift left.
        float leftCharacterWidth;       // The width of the ean/upc left hand character if has one.
        float supplimentWidth;			// Width of the add on barcode.
        float symbolHeight;				// Total barcode height including text (if visible) in scale units.
        float symbolWidth;				// Total barcode width including any Supplementry in scale units.
        SKFont textFont;                // Font used to render the human readable text.
        SKSize currentTextSize;         // Size of the current barcode text(human readable text).
        bool isCompositeSymbol;         // True if the symbol is a composite symbol.
        float firstBarXOffset;          // X Offset to the render the first bar/element of the symbol.
        int barsPerCharacter;           // The number of bars for one character for the current symbol;
        bool isDot;

        // Barcode text elements.
        string barcodeText;			    // The barcode's text.
        string checkDigitText;			// The check digit(s).
        string humanReadableText;       // The text to display.

        // EAN/UPC text elements.
        string leftHandCharacter;
        string leftHandText;
        string rightHandText;
        string rightHandCharacter;
        string supplementText;

        /* private static string[] SymbolNames = {
                 "Code One", "Code 39", "Code 39 Extended", "LOGMARS", "Code 32 (Italian Pharmacode)", "Pharmazentral Nummer", "Pharmacode", "Pharmacode 2-Track",
                 "Code 93", "Channel Code", "Telepen", "Telepen Numeric","Code 128", "EAN-14", "SSCC 18", "Code 2of5 Standard",
                 "Code 2of5 Interleaved", "Code 2of5 Matrix", "Code 2of5 IATA", "Code 2of5 Data Logic", "ITF-14", "Deutsche Post Identcode",
                 "Deutshe Post Leitcode", "Codabar", "MSI Plessey", "UK Plessey",
                 "Code 11", "ISBN", "EAN-13", "EAN-8", "UPC-A", "UPC-E", "Databar Omnidirectional", "Databar Omnidirectional Stacked",
                 "Databar Stacked", "Databar Truncated", "Databar Limited", "Databar Expanded", "Databar Expanded Stacked", "Data Matrix",
                 "QR Code", "Micro QR Code", "Aztec Code", "Aztec Runes", "Maxicode", "PDF 417", "PDF 417 Truncated",
                 "Micro PDF 417", "Aus Post Standard", "Aus Post Reply Paid", "Aus Post Redirect", "Aus Post Routing", "USPS Intelligent Mail", 
                 "PostNet Code", "Planet Code", "Korean Postal", "Facing Indentifcation Mark (FIM)", "Royal Mail 4 State", "Dutch Post (KIX)",
                 "DAFT Code", "Flattermarken", "Japanese Postal", "Codablock-F", "Code 16K", "DotCode", "Grid Matrix", "Code 49", "Han Xin Code" };*/

        #endregion

        #region Barcode Properties

        /// <summary>
        /// Gets or sets the font of the human readable text to be displayed with the sysmbol.
        /// </summary>
        public SKFont Font
        {
            get { return mFont; }
            set { mFont = value; }
        }

        /// <summary>
        /// Gets or Sets the barcode text to be encoded.
        /// </summary>
        public string BarcodeMessage
        {
            get { return barcodeMessage; }
            set { barcodeMessage = value; }
        }

        /// <summary>
        /// Gets or sets the EAN or UPC supplement text.
        /// </summary>
        public string SupplementMessage
        {
            get { return mSupplementMessage; }
            set { mSupplementMessage = value; }
        }

        /// <summary>
        /// Gets or sets the text to be encoded as the composited part of the symbol.
        /// </summary>
        public string CompositeMessage
        {
            get { return mCompositeMessage; }
            set
            {
                mCompositeMessage = value;
                if (String.IsNullOrEmpty(mCompositeMessage))
                    isCompositeSymbol = false;

                else
                    isCompositeSymbol = true;
            }
        }

        /// <summary>
        /// Gets or sets the mode for the composite part of the symbol.
        /// </summary>
        public CompositeMode CompositeMode
        {
            get { return mCompositeMode; }
            set { mCompositeMode = value; }
        }

        /// <summary>
        /// Gets or sets the encoding mode as Standard, GS1 or HIBC.
        /// </summary>
        public EncodingMode EncodingMode
        {
            get { return mEncodingMode; }
            set { mEncodingMode = value; }
        }

        /// <summary>
        /// Gets or sets to barcodes element width in millimeters.(X dimension)
        /// </summary>
        public float ElementXDimension
        {
            get { return mXDimension; }
            set { mXDimension = value; }
        }

        /// <summary>
        /// Gets or sets the multiplier for XDimension.
        /// </summary>
        public float Multiplier
        {
            get { return mMultiplier; }
            set
            {
                value = (float)Math.Round(value, 2);
                if (value < 0.8f)
                    value = 0.8f;

                else if (value > 15.0f)
                    value = 15;

                mMultiplier = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the barcode.
        /// </summary>
        public float BarcodeHeight
        {
            get { return mBarcodeHeight; }
            set { mBarcodeHeight = (float)Math.Round(value, 2); }
        }

        /// <summary>
        /// Gets or sets the margin between the barcode and the text.
        /// </summary>
        public float TextMargin
        {
            get { return mTextMargin; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 5.0f)
                    value = 5.0f;

                mTextMargin = (float)Math.Round(value, 2);
            }
        }

        /// <summary>
        /// Get or sets the barcodes text alignment.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return mTextAlignment; }
            set { mTextAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the text position as above or below the barcode.
        /// </summary>
        public TextPosition TextPosition
        {
            get { return mTextPosition; }
            set { mTextPosition = value; }
        }

        /// <summary>
        /// Gets or sets the text visiblity status.
        /// </summary>
        public bool TextVisible
        {
            get { return mTextVisible; }
            set { mTextVisible = value; }
        }

        /// <summary>
        /// Determines if the check digit is included in the human readable text.
        /// </summary>
        public bool ShowCheckDigit
        {
            get { return mShowCheckDigit; }
            set { mShowCheckDigit = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the barcode in degrees.
        /// </summary>
        public int Rotation
        {
            get { return mRotation; }
            set
            {
                // Only accept rotation in multiples of 90 degrees.
                if (value < 90 || value > 359)
                    mRotation = 0;

                if (value >= 90 && value < 180)
                    mRotation = 90;

                if (value >= 180 && value < 270)
                    mRotation = 180;

                if (value >= 270 && value < 360)
                    mRotation = 270;

            }
        }

        /// <summary>
        /// Gets or sets the barcode fore color.
        /// </summary>
        public SKColor BarcodeColor
        {
            get { return mBarcodeColor; }
            set { mBarcodeColor = value; }
        }

        /// <summary>
        /// Gets or sets the barcode text fore color.
        /// </summary>
        public SKColor BarcodeTextColor
        {
            get { return mTextColor; }
            set { mTextColor = value; }
        }

        /// <summary>
        /// Gets or sets the optional check digit status.
        /// </summary>
        public bool OptionalCheckDigit
        {
            get { return mOptionalCheckDigit; }
            set { mOptionalCheckDigit = value; }
        }

        /// <summary>
        /// Gets or sets the check digit type for Interleaved 2of5.
        /// </summary>
        public I2of5CheckDigitType I2of5CheckDigitType
        {
            get { return mI2of5CheckDigitType; }
            set { mI2of5CheckDigitType = value; }
        }

        /// <summary>
        /// Gets or sets the check digit type for MSI symbol.
        /// </summary>
        public MSICheckDigitType MSICheckDigitType
        {
            get { return mMSICheckDigitType; }
            set { mMSICheckDigitType = value; }
        }

        /// <summary>
        /// Gets or sets the number of Code11 check digits.
        /// </summary>
        public int Code11CheckDigits
        {
            get { return mNumberOfCheckDigits; }
            set
            {
                if (value < 1)
                    value = 1;

                else if (value > 2)
                    value = 2;

                mNumberOfCheckDigits = value;
            }
        }

        /// <summary>
        /// Gets or sets the style of the ITF14 bearer bars.
        /// </summary>
        public ITF14BearerStyle ITF14BearerStyle
        {
            get { return mITF14BearerStyle; }
            set { mITF14BearerStyle = value; }
        }

        /// <summary>
        /// Gets or sets the ECI mode.
        /// </summary>
        public int ECIMode
        {
            get { return mECIMode; }
            set { mECIMode = value; }
        }

        /// <summary>
        /// Gets or sets Australia Post encoding mode.
        /// </summary>
        public AusPostEncoding AusPostEncoding
        {
            get { return mAusPostEncoding; }
            set { mAusPostEncoding = value; }
        }

        /// <summary>
        /// Gets or sets Maxicode mode.
        /// </summary>
        public MaxicodeMode MaxicodeMode
        {
            get { return mMaxicodeMode; }
            set { mMaxicodeMode = value; }
        }

        /// <summary>
        /// Gets or sets QR Code version.
        /// </summary>
        /// <remarks> 0 = Automatic mode</remarks>
        public int QRVersion
        {
            get { return mQRCodeVersion; }
            set
            {
                // 0 = Auto version
                if (symbolId == Symbology.MicroQRCode)  // Valid = 1 to 4
                {
                    if (value < 0)
                        value = 0;

                    if (value > 4)
                        value = 4;
                }

                else if (symbolId == ZintNet.Symbology.RectangularMicroQRCode)   // Valid = 1 to 38
                {
                    if (value < 0)
                        value = 0;

                    if (value > 38)
                        value = 38;
                }

                else   // Valid = 1 to 40;
                {
                    if (value < 0)
                        value = 0;

                    if (value > 40)
                        value = 40;
                }

                mQRCodeVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets QR code error correction level.
        /// </summary>
        public QRCodeEccLevel QRCodeEccLevel
        {
            get { return mQRCodeEccLevel; }
            set { mQRCodeEccLevel = value; }
        }

        /// <summary>
        /// Get or sets the Datamatrix symbol size.
        /// </summary>
        /// <remarks>0 = Automatic mode</remarks>
        public DataMatrixSize DataMatrixSize
        {
            get { return mDataMatrixSize; }
            set { mDataMatrixSize = value; }
        }

        /// <summary>
        /// Gets or sets permission to use square only Datamatrix symbols.
        /// </summary>
        public bool DataMatrixSquare
        {
            get { return mDataMatrixSquare; }
            set { mDataMatrixSquare = value; }
        }

        /// <summary>
        /// Get or sets permission to use Datamatrix rectangular extensions.
        /// </summary>
        public bool DataMatrixRectExtn
        {
            get { return mDataMatrixRectExtn; }
            set { mDataMatrixRectExtn = value; }
        }

        /// <summary>
        /// Gets or sets the Aztec Code size.
        /// </summary>
        /// <remarks> 0 = Automatic Mode</remarks>
        public int AztecSize
        {
            get { return mAztecSize; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 36)
                    value = 36;

                mAztecSize = value;
            }
        }

        /// <summary>
        /// Gets or sets Aztec code error correction level.
        /// </summary>
        public int AztecErrorLevel
        {
            get { return mAztecEccLevel; }
            set
            {
                if (value < -1 || value > 4)
                    value = -1;

                mAztecEccLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the Han Xin Code version.
        /// </summary>
        /// <remarks> 0 = Automatic Mode</remarks>
        public int HanXinVersion
        {
            get { return mHanXinVersion; }
            set
            {
                if (value < 0 || value > 84)
                    value = 0;

                mHanXinVersion = value;
            }
        }

        /// <summary>
        /// Gets or set the Han Xin Code error correction level.
        /// </summary>
        public int HanXinErrorLevel
        {
            get { return mHanXinEccLevel; }
            set
            {
                if (value < -1 || value > 4)
                    value = -1;

                mHanXinEccLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the Ultracode compression.
        /// </summary>
        /// <remarks> 0 = No compression</remarks>
        public int UltracodeCompression
        {
            get { return mUltracodeCompression; }
            set
            {
                if (value != 0 || value != 128)
                    value = 0;

                mUltracodeCompression = value;
            }
        }

        /// <summary>
        /// Gets or sets Ultracode error correction level.
        /// </summary>
        public int UltracodeErrorLevel
        {
            get { return mUltracodeEccLevel; }
            set
            {
                if (value < -1 || value > 6)
                    value = -1;

                mUltracodeEccLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of PDF417 columns.
        /// </summary>
        public int PDF417Columns
        {
            get { return mPDF417Columns; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 20)
                    value = 20;

                mPDF417Columns = value;
            }
        }

        /// <summary>
        /// Gets or sets the PDF417 error correction level.
        /// </summary>
        public int PDF417ErrorLevel
        {
            get { return mPDF417ErrorLevel; }
            set
            {
                if (value < -1)
                    value = -1;

                if (value > 8)
                    value = 8;

                mPDF417ErrorLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the row height of the PDF417 symbol.
        /// </summary>
        public int PDF417RowHeight
        {
            get { return mPDF417RowHeight; }

            set
            {
                if (value < 1)
                    value = 1;

                if (value > 5)
                    value = 5;

                mPDF417RowHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of Databar Expanded segments.
        /// </summary>
        public int DatabarExpandedSegments
        {
            get { return mDatabarExpandedSegments; }
            set
            {
                if (value < 2 || value > 22 || value % 2 != 0)
                    value = 0;

                mDatabarExpandedSegments = value;
            }
        }

        /// <summary>
        /// Gets or set the number of Channel Code channels.
        /// </summary>
        public int ChannelCodeLevel
        {
            get { return mChannelCodeLevel; }
            set
            {
                // 0 = automatic.
                if ((value > 0 && value < 3) || value > 8)
                    value = 0;

                mChannelCodeLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of Codablock rows.
        /// </summary>
        public int CodablockFRows
        {
            get { return mCodablockFRows; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 44)
                    value = 44;

                mCodablockFRows = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of codablock columns.
        /// </summary>
        public int CodablockFColumns
        {
            get { return mCodablockFColumns; }
            set
            {
                if (value < 6)
                    value = 0;

                if (value > 66)
                    value = 66;

                mCodablockFColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of Dot Code columns.
        /// </summary>
        public int DotCodeColumns
        {
            get { return mDotCodeColumns; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 50)
                    value = 50;

                mDotCodeColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets the Code One size.
        /// </summary>
        public int CodeOneSize
        {
            get { return mCodeOneSize; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 10)
                    value = 10;

                mCodeOneSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the Grid Matrix version.
        /// </summary>
        public int GridMatixVersion
        {
            get { return mGridMatrixVersion; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value > 13)
                    value = 13;

                mGridMatrixVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the Grid Matrix error correction level.
        /// </summary>
        public int GridMatrixEccLevel
        {
            get { return mGridMatrixEccLevel; }
            set
            {
                if (value < -1 || value < 5)
                    value = -1;

                mGridMatrixEccLevel = value;
            }
        }

        #endregion

        #region	Read Only Properties

        /// <summary>
        /// Gets the current symbol ID.
        /// </summary>
        public Symbology Symbology
        {
            get { return symbolId; }
        }

        /// <summary>
        /// Returns true if a symbol was successfully generated.
        /// </summary>
        public bool IsValid
        {
            get { return isValidFlag; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instantance of the ZintNetLib object.
        /// </summary>
        /// <remarks>Constructor</remarks>
        public ZintNetLib()
        {
            Initialize();
        }

        /// <summary>
        /// Dispose method for the ZintNetLib object.
        /// </summary>
        ~ZintNetLib()
        {
            Dispose(false);
        }

        // Flag: Has Dispose already been called? 
        bool disposed = false;

        /// <summary>
        /// Public implementation of Dispose method for the ZintNetLib object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose method for the ZintNetLib object. 
        /// </summary>
        /// <param name="disposing">true if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                if (mFont != null)
                    mFont.Dispose();

                if (textFont != null)
                    textFont.Dispose();
            }

            // Free any unmanaged objects here. 
            disposed = true;
        }

        /// <summary>
        /// Gets encoded data as a string.
        /// </summary>
        /// <returns></returns>
        public override System.String ToString()
        {
            StringBuilder binaryString = new StringBuilder();
            IEnumerator<SymbolData> enumerator = encodedData.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SymbolData symbolData = enumerator.Current;
                byte[] rowData = symbolData.GetRowData();
                foreach (byte byteValue in rowData)
                {
                    if (symbolId == Symbology.Ultracode)
                        binaryString.Append((char)byteValue);

                    else if (byteValue == 0 || byteValue == 1)
                        binaryString.Append(byteValue);
                }

                binaryString.Append(Environment.NewLine);
            }

            return binaryString.ToString();
        }

        /// <summary>
        /// Creates a new barcode.
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <param name="barcodeMessage">barcode message</param>
        public void CreateBarcode(string symbolName, string barcodeMessage)
        {
            CreateBarcode(GetSymbolId(symbolName), barcodeMessage);
        }

        /// <summary>
        /// Creates a new barcode.
        /// </summary>
        /// <param name="symbolId">symbol id number</param>
        /// <param name="barcodeMessage">barcode message</param>
        public void CreateBarcode(Symbology symbolId, string barcodeMessage)
        {
            try
            {
                if (String.IsNullOrEmpty(barcodeMessage))
                    throw new System.ArgumentException("Parameter: barcodeMessage can not be null or empty.");

                this.symbolId = symbolId;
                this.barcodeMessage = barcodeMessage;
                isDot = (symbolId == Symbology.DotCode) ? true : false;
                isEanUpc = IsEanUpc();
                EncodeData();
            }

            catch (Exception ex)
            {
                isValidFlag = false;
                throw new ZintNetDLLException("", ex);
            }
        }

        /// <summary>
        /// Gets the symbol ID from the symbol name.
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <returns>ID number</returns>
        public static Symbology GetSymbolId(string symbolName)
        {
            Symbology id = SymbolDictionary.GetSymbolId(symbolName);
            if (id == Symbology.Invalid)
                throw new UnknownSymbolException(symbolName);

            return id;
        }

        /// <summary>
        /// Gets the names of supported symbologies.
        /// </summary>
        /// <returns>array containing symbology names.</returns>
        public static string[] GetSymbolNames()
        {
            string[] symbolNames = SymbolDictionary.GetSymbolNames();
            return symbolNames;
        }

        /// <summary>
        /// Renders the barcode to a user supplied SkiaSharp canvas.
        /// </summary>
        /// <param name="canvas">SKCanvas to render the barcode</param>
        /// <param name="startXY">point to start the rendering (in pixels)</param>
        /// <param name="dpiX">Horizontal DPI for pixel-to-mm conversion</param>
        /// <param name="dpiY">Vertical DPI for pixel-to-mm conversion</param>
        public void DrawBarcode(SKCanvas canvas, SKPoint startXY, float dpiX, float dpiY)
        {
            float totalSymbolHeight = 0.0f;
            float elementWidth = mXDimension * mMultiplier;
            float elementHeight = 0;
            float barHeight = 0;
            int numberOfRows = 0;
            int rowWidth;
            float barStartY = 0;
            float rowStart;
            float nextBar;
            SymbolData symbolData;
            byte[] rowData;

            // Convert the start point to millimeters.
            startXY.X = DisplayMetrics.Convert.PixelsToMillimeter(startXY.X, dpiX);
            startXY.Y = DisplayMetrics.Convert.PixelsToMillimeter(startXY.Y, dpiY);

            // Save the canvas state
            canvas.Save();
            try
            {
                GetTotalSymbolSize();
                // Apply transformations.
                canvas.Translate(startXY.X + (symbolWidth / 2), startXY.Y + (symbolHeight / 2));
                canvas.RotateDegrees(mRotation);
                canvas.Translate(-(symbolWidth / 2), -(symbolHeight / 2));

                // Draw maxicode.
                if (symbolId == Symbology.MaxiCode)
                {
                    using (var pen = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Stroke, StrokeWidth = 0.67f * mMultiplier, IsAntialias = true })
                    using (var brush = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Fill, IsAntialias = true })
                    {
                        float centerX = 13.64f * mMultiplier;
                        float centerY = 13.43f * mMultiplier;
                        float innerRadius = 0.85f * mMultiplier;
                        float centerRadius = 2.20f * mMultiplier;
                        float outerRadius = 3.54f * mMultiplier;
                        canvas.DrawOval(new SKRect(centerX - innerRadius, centerY - innerRadius, centerX + innerRadius, centerY + innerRadius), pen);
                        canvas.DrawOval(new SKRect(centerX - centerRadius, centerY - centerRadius, centerX + centerRadius, centerY + centerRadius), pen);
                        canvas.DrawOval(new SKRect(centerX - outerRadius, centerY - outerRadius, centerX + outerRadius, centerY + outerRadius), pen);

                        int row = 0;
                        IEnumerator<SymbolData> enumerator = encodedData.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            symbolData = enumerator.Current;
                            rowData = symbolData.GetRowData();
                            rowWidth = symbolData.RowCount;
                            for (int column = 0; column < rowWidth; column++)
                            {
                                if (rowData[column] == 1)
                                {
                                    float rowOffset = (((row & 1) == 1) ? 1.32f : 0.88f);
                                    SKRect hexRect = new SKRect(
                                        ((column * 0.88f) + rowOffset) * mMultiplier,
                                        ((row * 0.76f) + 0.76f) * mMultiplier,
                                        ((column * 0.88f) + rowOffset) * mMultiplier + 0.76f * mMultiplier,
                                        ((row * 0.76f) + 0.76f) * mMultiplier + 0.88f * mMultiplier);
                                    SKPoint[] hexagonPoints = new SKPoint[] {
                                        new SKPoint(hexRect.Left + hexRect.Width * 0.5f, hexRect.Top),
                                        new SKPoint(hexRect.Right, hexRect.Top + hexRect.Height * 0.25f),
                                        new SKPoint(hexRect.Right, hexRect.Top + hexRect.Height * 0.75f),
                                        new SKPoint(hexRect.Left + hexRect.Width * 0.5f, hexRect.Bottom),
                                        new SKPoint(hexRect.Left, hexRect.Top + hexRect.Height * 0.75f),
                                        new SKPoint(hexRect.Left, hexRect.Top + hexRect.Height * 0.25f)
                                    };
                                    using (var path = new SKPath())
                                    {
                                        path.MoveTo(hexagonPoints[0]);
                                        for (int i = 1; i < hexagonPoints.Length; i++)
                                            path.LineTo(hexagonPoints[i]);
                                        path.Close();
                                        canvas.DrawPath(path, brush);
                                    }
                                }
                            }
                            row++;
                        }
                    }
                    totalSymbolHeight = symbolHeight;
                }
                else
                {
                    barStartY = 0.0f;
                    if (mTextVisible && mTextPosition == TextPosition.AboveBarcode && !isEanUpc && symbolId != Symbology.ITF14)
                        barStartY = (float)Math.Ceiling(currentTextSize.Height + mTextMargin);

                    using (var barBrush = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Fill })
                    {
                        numberOfRows = encodedData.Count;
                        IEnumerator<SymbolData> enumerator = encodedData.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            symbolData = enumerator.Current;
                            if (symbolData.RowHeight != 0.0f)
                                elementHeight = symbolData.RowHeight * elementWidth;
                            else
                                elementHeight = mBarcodeHeight;
                            barHeight = elementHeight;
                            rowStart = barStartY;
                            nextBar = firstBarXOffset;
                            rowData = symbolData.GetRowData();
                            rowWidth = symbolData.RowCount;
                            for (int i = 0; i < rowWidth; i++)
                            {
                                byte barBit = rowData[i];
                                if (barBit > 1)
                                {
                                    if (isEanUpc)
                                    {
                                        switch (barBit)
                                        {
                                            case (byte)'N':
                                                rowStart = barStartY;
                                                barHeight = elementHeight;
                                                break;
                                            case (byte)'G':
                                                rowStart = barStartY;
                                                barHeight = elementHeight + (currentTextSize.Height / 2);
                                                break;
                                            case (byte)'S':
                                                rowStart = barStartY + currentTextSize.Height;
                                                barHeight = elementHeight - (currentTextSize.Height / 2);
                                                break;
                                        }
                                        continue;
                                    }
                                    if (symbolId == Symbology.Ultracode)
                                    {
                                        switch ((char)barBit)
                                        {
                                            case 'W': barBrush.Color = SKColors.White; break;
                                            case 'C': barBrush.Color = SKColors.Cyan; break;
                                            case 'B': barBrush.Color = SKColors.Blue; break;
                                            case 'M': barBrush.Color = SKColors.Magenta; break;
                                            case 'R': barBrush.Color = SKColors.Red; break;
                                            case 'Y': barBrush.Color = SKColors.Yellow; break;
                                            case 'G': barBrush.Color = SKColors.Lime; break;
                                            case 'K': barBrush.Color = SKColors.Black; break;
                                        }
                                        barBit = 1;
                                    }
                                }
                                if (barBit == 1)
                                {
                                    if (isDot)
                                        canvas.DrawOval(new SKRect(nextBar, rowStart, nextBar + elementWidth, rowStart + barHeight), barBrush);
                                    else
                                        canvas.DrawRect(new SKRect(nextBar, rowStart, nextBar + elementWidth, rowStart + barHeight), barBrush);
                                }
                                nextBar += elementWidth;
                            }
                            barStartY += barHeight;
                            totalSymbolHeight += elementHeight;
                        }
                    }
                }
                // CodablockF bearer and binding bars
                if (symbolId == Symbology.CodablockF)
                {
                    using (var pen = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Stroke, StrokeWidth = elementWidth })
                    {
                        float startY = 0.0f;
                        canvas.DrawLine(new SKPoint(0, startY + (elementWidth / 2)), new SKPoint(symbolWidth, startY + (elementWidth / 2)), pen);
                        if (numberOfRows > 1)
                        {
                            for (int r = 0; r < numberOfRows - 1; r++)
                            {
                                startY += elementHeight;
                                canvas.DrawLine(new SKPoint(barsPerCharacter * elementWidth, startY), new SKPoint(symbolWidth - ((barsPerCharacter + 3) * elementWidth), startY), pen);
                            }
                        }
                        startY += elementHeight;
                        canvas.DrawLine(new SKPoint(0, startY + (elementWidth / 2)), new SKPoint(symbolWidth, startY + (elementWidth / 2)), pen);
                    }
                    totalSymbolHeight += elementWidth;
                }
                // Code 16K and Code49 bearer and binding bars
                if (symbolId == Symbology.Code16K || symbolId == Symbology.Code49)
                {
                    using (var pen = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Stroke, StrokeWidth = elementWidth })
                    {
                        float startY = 0.0f;
                        float startX = 0.0f;
                        canvas.DrawLine(new SKPoint(startX, startY + (elementWidth / 2)), new SKPoint(symbolWidth, startY + (elementWidth / 2)), pen);
                        if (numberOfRows > 1)
                        {
                            for (int r = 0; r < numberOfRows - 1; r++)
                            {
                                startY += elementHeight;
                                canvas.DrawLine(new SKPoint(firstBarXOffset, startY), new SKPoint(symbolWidth - (2.0f * elementWidth), startY), pen);
                            }
                        }
                        startY += elementHeight;
                        canvas.DrawLine(new SKPoint(startX, startY + (elementWidth / 2)), new SKPoint(symbolWidth, startY + (elementWidth / 2)), pen);
                    }
                    totalSymbolHeight += elementWidth;
                }
                // ITF14 bearer bars or rectangle
                if (symbolId == Symbology.ITF14)
                {
                    if (symbolId == Symbology.ITF14 && mITF14BearerStyle != ITF14BearerStyle.None)
                    {
                        using (var pen = new SKPaint { Color = mBarcodeColor, Style = SKPaintStyle.Stroke, StrokeWidth = bearerWidth })
                        {
                            if (mITF14BearerStyle == ITF14BearerStyle.Rectangle)
                            {
                                SKPoint[] bearerRectangle = new SKPoint[] {
                                    new SKPoint(bearerWidth / 2, bearerWidth / 2),
                                    new SKPoint(symbolWidth - (bearerWidth / 2), bearerWidth / 2),
                                    new SKPoint(symbolWidth - (bearerWidth / 2), elementHeight + (bearerWidth / 2)),
                                    new SKPoint(bearerWidth / 2, elementHeight + (bearerWidth / 2)),
                                    new SKPoint(bearerWidth / 2, 0)
                                };
                                for (int i = 0; i < bearerRectangle.Length - 1; i++)
                                    canvas.DrawLine(bearerRectangle[i], bearerRectangle[i + 1], pen);
                            }
                            else
                            {
                                canvas.DrawLine(new SKPoint(0, bearerWidth / 2), new SKPoint(symbolWidth, bearerWidth / 2), pen);
                                canvas.DrawLine(new SKPoint(0, elementHeight + (bearerWidth / 2)), new SKPoint(symbolWidth, elementHeight + (bearerWidth / 2)), pen);
                            }
                        }
                        totalSymbolHeight += bearerWidth;
                    }
                }
                // Render the barcode text
                if (isEanUpc && mTextVisible)
                {
                    DrawEanUpcText(canvas, totalSymbolHeight);
                }
                else if (mTextVisible)
                {
                    DrawLinearText(canvas, totalSymbolHeight);
                }
            }
            catch (Exception ex)
            {
                throw new ZintNetDLLException("Error rendering barcode.", ex);
            }
            finally
            {
                canvas.Restore();
            }
        }

        // SkiaSharp version of DrawEanUpcText
        private void DrawEanUpcText(SKCanvas canvas, float totalSymbolHeight)
        {
            float centreAdjust;
            float textStartX = 0.0f;
            float textStartY = totalSymbolHeight;
            float elementWidth = mXDimension * mMultiplier;
            float characterSegment = barsPerCharacter * elementWidth;
            int leftGuardBars = 4;
            int centerGuardBars = 4;
            int rightGuardBars = 4;

            if (symbolId == Symbology.UPCA)
                leftGuardBars = 11;

            float leftHandTextPosition = ((linearShiftCount + leftGuardBars) * elementWidth) + firstBarXOffset;

            using (var paint = new SKPaint { Color = mTextColor, Typeface = textFont.Typeface, TextSize = textFont.Size, IsAntialias = true })
            {
                if (!string.IsNullOrEmpty(leftHandCharacter))
                {
                    textStartX = 0.0f;
                    if (isCompositeSymbol)
                        textStartX += elementWidth;
                    canvas.DrawText(leftHandCharacter, textStartX, textStartY + -paint.FontMetrics.Ascent, paint);
                }

                textStartX = leftHandTextPosition;
                // Render the left hand and right hand text equally spaced between the guard bars.
                for (int i = 0; i < leftHandText.Length; i++)
                {
                    string textCharacter = leftHandText[i].ToString();
                    float characterWidth = paint.MeasureText(textCharacter);
                    centreAdjust = (characterSegment - characterWidth) / 2;
                    canvas.DrawText(textCharacter, textStartX + centreAdjust, textStartY + -paint.FontMetrics.Ascent, paint);
                    textStartX += characterSegment;
                }

                if (!string.IsNullOrEmpty(rightHandText))
                {
                    textStartX = leftHandTextPosition + (characterSegment * leftHandText.Length) + (centerGuardBars * elementWidth);
                    for (int i = 0; i < leftHandText.Length; i++)
                    {
                        string textCharacter = rightHandText[i].ToString();
                        float characterWidth = paint.MeasureText(textCharacter);
                        centreAdjust = (characterSegment - characterWidth) / 2;
                        canvas.DrawText(textCharacter, textStartX + centreAdjust, textStartY + -paint.FontMetrics.Ascent, paint);
                        textStartX += characterSegment;
                    }
                }

                // Move the text start X point to the end of the symbol.
                if (symbolId == Symbology.UPCA)
                    rightGuardBars = 11;
                else if (symbolId == Symbology.UPCE)
                    rightGuardBars = 6;

                textStartX += (rightGuardBars * elementWidth);
                if (isCompositeSymbol)
                    textStartX -= elementWidth;

                if (!string.IsNullOrEmpty(rightHandCharacter))
                    canvas.DrawText(rightHandCharacter, textStartX, textStartY + -paint.FontMetrics.Ascent, paint);

                if (!string.IsNullOrEmpty(supplementText))
                {
                    float textWidth = paint.MeasureText(supplementText);
                    textStartX += (SymbolEncoder.supplementMargin * elementWidth) + (((supplimentWidth * elementWidth) - textWidth) / 2);
                    textStartY = totalSymbolHeight - mBarcodeHeight;
                    canvas.DrawText(supplementText, textStartX, textStartY + -paint.FontMetrics.Ascent, paint);

                    // Draw the quiet space character.
                    textStartX = (symbolWidth - leftCharacterWidth + elementWidth);
                    if (isCompositeSymbol)
                        textStartX -= elementWidth;

                    canvas.DrawText(">", textStartX, textStartY + -paint.FontMetrics.Ascent, paint);
                }
            }
        }

        // SkiaSharp version of DrawLinearText
        private void DrawLinearText(SKCanvas canvas, float totalSymbolHeight)
        {
            TextAlignment textAlignment = mTextAlignment;
            float textStartX = 0.0f;
            float textStartY = (mTextPosition == TextPosition.AboveBarcode) ? 0.0f : totalSymbolHeight + mTextMargin;
            float textWidth = symbolWidth;
            float textHeight = symbolHeight;

            // Override the "Stretched" alignment if the text is wider than the barcode.
            if (textAlignment == TextAlignment.Stretched && currentTextSize.Width > symbolWidth)
                textAlignment = TextAlignment.Center;

            using (var paint = new SKPaint { Color = mTextColor, Typeface = textFont.Typeface, TextSize = textFont.Size, IsAntialias = true })
            {
                if (textAlignment == TextAlignment.Stretched)
                {
                    DrawTextStretched(canvas, paint, textStartX, textStartY);
                }
                else
                {
                    float x = textStartX;
                    switch (textAlignment)
                    {
                        case TextAlignment.Center:
                            x = textStartX + (textWidth - currentTextSize.Width) / 2f;
                            break;
                        case TextAlignment.Left:
                            x = textStartX;
                            break;
                        case TextAlignment.Right:
                            x = textStartX + (textWidth - currentTextSize.Width);
                            break;
                    }
                    // Draw the text baseline at textStartY + -paint.FontMetrics.Ascent
                    canvas.DrawText(humanReadableText, x, textStartY + -paint.FontMetrics.Ascent, paint);
                }
            }
        }

        // SkiaSharp version of DrawTextStretched
        private void DrawTextStretched(SKCanvas canvas, SKPaint paint, float textStartX, float textStartY)
        {
            // Stretch the barcode text spaced equally across the width of the barcode.
            int textLength = humanReadableText.Length;
            if (textLength == 0) return;
            float characterSegment = symbolWidth / textLength;
            float x = textStartX;
            for (int i = 0; i < textLength; i++)
            {
                string textCharacter = humanReadableText[i].ToString();
                float characterWidth = paint.MeasureText(textCharacter);
                float centreAdjust = (characterSegment - characterWidth) / 2f;
                canvas.DrawText(textCharacter, x + centreAdjust, textStartY + -paint.FontMetrics.Ascent, paint);
                x += characterSegment;
            }
        }

        /// <summary>
        /// Initialise property and general fields to default values.
        /// </summary>
        private void Initialize()
        {
            mFont = new SKFont(SKTypeface.Default, 10.0f);
            mBarcodeHeight = 20.0f;
            mMultiplier = 1.0f;
            mXDimension = 0.264583f;    // 1 pixel.
            mRotation = 0;
            mNumberOfCheckDigits = 2;
            mITF14BearerStyle = ITF14BearerStyle.Rectangle;
            mBarcodeColor = SKColors.Black;
            mTextMargin = 0.0f;
            mTextColor = SKColors.Black;
            mTextAlignment = TextAlignment.Center;
            mTextPosition = TextPosition.UnderBarcode;
            mTextVisible = true;
            mOptionalCheckDigit = false;
            mShowCheckDigit = false;
            mMSICheckDigitType = MSICheckDigitType.Mod10;
            mI2of5CheckDigitType = I2of5CheckDigitType.USS;
            mSupplementMessage = String.Empty;
            mEncodingMode = EncodingMode.Standard;
            mECIMode = 0;
            mChannelCodeLevel = 0;
            mCodablockFRows = 0;
            mCodablockFColumns = 0;
            mDotCodeColumns = 0;
            mQRCodeEccLevel = (QRCodeEccLevel) - 1;
            mQRCodeVersion = 0;
            mDataMatrixSize = DataMatrixSize.Automatic;
            mDataMatrixSquare = true;
            mDataMatrixRectExtn = false;
            mAztecSize = 0;
            mAztecEccLevel = -1;
            mHanXinVersion = 0;
            mHanXinEccLevel = -1;
            mUltracodeCompression = 0;
            mUltracodeEccLevel = -1;
            mGridMatrixVersion = 0;
            mGridMatrixEccLevel = -1;
            mCodeOneSize = 0;
            mPDF417Columns = 0;
            mPDF417ErrorLevel = -1;
            mPDF417RowHeight = 2;
            mDatabarExpandedSegments = 0;
            mCompositeMessage = string.Empty;
            mAusPostEncoding = AusPostEncoding.Numeric;
            mCompositeMode = CompositeMode.CCA;
            mMaxicodeMode = MaxicodeMode.Mode4;
            isValidFlag = false;
            isEanUpc = IsEanUpc();
            isDot = false;
            linearShiftCount = 0;
            leftCharacterWidth = 0.0f;
            firstBarXOffset = 0.0f;
            textFont = new SKFont(SKTypeface.Default, 10.0f);
        }

        /// <summary>
        /// Calculates the final dimensions of the symbol including any displayed text.
        /// </summary>
        /// <param name="dpiX">Horizontal DPI for pixel-to-mm conversion</param>
        /// <param name="dpiY">Vertical DPI for pixel-to-mm conversion</param>
        /// <returns>returns the symbol dimensions in pixels as SKSizeI</returns>
        public SKSizeI SymbolSize(float dpiX, float dpiY)
        {
            var symbolSize = new SKSizeI(0, 0);
            if (isValidFlag)
            {
                GetTotalSymbolSize();
                int widthPx = (int)Math.Ceiling(DisplayMetrics.Convert.MillimetersToPixels(symbolWidth, dpiX));
                int heightPx = (int)Math.Ceiling(DisplayMetrics.Convert.MillimetersToPixels(symbolHeight, dpiY));
                symbolSize = new SKSizeI(widthPx, heightPx);
            }
            return symbolSize;
        }

        /// <summary>
        /// Encodes the barcode symbol data.
        /// </summary>
        private void EncodeData()
        {
            isValidFlag = false;
            SymbolEncoder encoder = null;
            switch (symbolId)
            {
                case Symbology.AusPostStandard:
                case Symbology.AusPostReplyPaid:
                case Symbology.AusPostRedirect:
                case Symbology.AusPostRouting:
                    encoder = new AusPostEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.RoyalMailMailmark:
                    encoder = new MailmarkEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.USPS:
                    encoder = new IntelligentMailEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.PostNet:
                case Symbology.Planet:
                case Symbology.KoreaPost:
                case Symbology.FIM:
                case Symbology.RoyalMail:
                case Symbology.KixCode:
                case Symbology.DaftCode:
                case Symbology.Flattermarken:
                case Symbology.JapanPost:
                    encoder = new PostalEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.Code32:
                case Symbology.PharmaZentralNummer:
                case Symbology.Code39:
                case Symbology.Code39Extended:
                case Symbology.LOGMARS:
                case Symbology.VINCode:
                    encoder = new Code39Encoder(symbolId, barcodeMessage, mOptionalCheckDigit, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.ChannelCode:
                    encoder = new ChannelCodeEncoder(symbolId, barcodeMessage, mChannelCodeLevel);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Telepen:
                case Symbology.TelepenNumeric:
                    encoder = new TelepenEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Pharmacode:
                case Symbology.Pharmacode2Track:
                    encoder = new PharmacodeEncoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.EAN14:
                case Symbology.SSCC18:
                case Symbology.Code128:
                    encoder = new Code128Encoder(symbolId, barcodeMessage, mCompositeMessage, mCompositeMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Code93:
                    encoder = new Code93Encoder(symbolId, barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.Standard2of5:
                case Symbology.Interleaved2of5:
                case Symbology.Matrix2of5:
                case Symbology.IATA2of5:
                case Symbology.DataLogic2of5:
                case Symbology.ITF14:
                case Symbology.DeutschePostIdentCode:
                case Symbology.DeutshePostLeitCode:
                    encoder = new Code2of5Encoder(symbolId, barcodeMessage, mOptionalCheckDigit, mI2of5CheckDigitType);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.Codabar:
                    encoder = new CodabarEncoder(barcodeMessage);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.MSIPlessey:
                case Symbology.UKPlessey:
                    encoder = new PlesseyEncoder(symbolId, barcodeMessage, mMSICheckDigitType);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.Code11:
                    encoder = new Code11Encoder(barcodeMessage, mOptionalCheckDigit, mNumberOfCheckDigits);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    checkDigitText = encoder.CheckDigitText;
                    break;

                case Symbology.ISBN:
                case Symbology.EAN13:
                case Symbology.EAN8:
                case Symbology.UPCA:
                case Symbology.UPCE:
                    encoder = new EANUPCEncoder(symbolId, barcodeMessage, mSupplementMessage, mCompositeMessage, mCompositeMode);
                    encodedData = encoder.EncodeData();
                    leftHandCharacter = encoder.LeftHandCharacter;
                    leftHandText = encoder.LeftHandText;
                    rightHandText = encoder.RightHandText;
                    rightHandCharacter = encoder.RightHandCharacter;
                    supplementText = encoder.SupplementText;
                    supplimentWidth = encoder.SupplimentBars;
                    barsPerCharacter = encoder.ElementsPerCharacter;
                    break;

                case Symbology.DatabarOmni:
                case Symbology.DatabarOmniStacked:
                case Symbology.DatabarTruncated:
                case Symbology.DatabarStacked:
                case Symbology.DatabarLimited:
                case Symbology.DatabarExpanded:
                case Symbology.DatabarExpandedStacked:
                    encoder = new DatabarEncoder(symbolId, barcodeMessage, mCompositeMessage, mCompositeMode, mDatabarExpandedSegments);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.QRCode:
                case Symbology.MicroQRCode:
                case Symbology.UPNQR:
                case Symbology.RectangularMicroQRCode:
                    encoder = new QRCodeEncoder(symbolId, barcodeMessage, mQRCodeVersion, mQRCodeEccLevel, mECIMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.DataMatrix:
                    encoder = new DataMatrixEncoder(symbolId, barcodeMessage, mDataMatrixSize, mDataMatrixSquare, mDataMatrixRectExtn, mECIMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.CodeOne:
                    encoder = new CodeOneEncoder(symbolId, barcodeMessage, mCodeOneSize, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Aztec:
                case Symbology.AztecRunes:
                    encoder = new AztecEncoder(symbolId, barcodeMessage, mAztecSize, mAztecEccLevel, mECIMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.MaxiCode:
                    encoder = new MaxiCodeEncoder(symbolId, barcodeMessage, mMaxicodeMode, mECIMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.PDF417:
                case Symbology.PDF417Truncated:
                case Symbology.MicroPDF417:
                    encoder = new PDF417Encoder(symbolId, barcodeMessage, mPDF417Columns, mPDF417ErrorLevel, mPDF417RowHeight, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.CodablockF:
                    encoder = new CodaBlockEncoder(symbolId, barcodeMessage, mCodablockFRows, mCodablockFColumns);
                    encodedData = encoder.EncodeData();
                    barsPerCharacter = encoder.ElementsPerCharacter;
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Code16K:
                    encoder = new Code16KEncoder(symbolId, barcodeMessage, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.DotCode:
                    encoder = new DotCodeEncoder(symbolId, barcodeMessage, mDotCodeColumns, mECIMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.GridMatrix:
                    encoder = new GridMatrixEncoder(symbolId, barcodeMessage, mGridMatrixVersion, mGridMatrixEccLevel, mECIMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Code49:
                    encoder = new Code49Encoder(symbolId, barcodeMessage, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.HanXin:
                    encoder = new HanXinEncoder(symbolId, barcodeMessage, mHanXinVersion, mHanXinEccLevel, mECIMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;

                case Symbology.Ultracode:
                    encoder = new UltraEncoder(symbolId, barcodeMessage, mUltracodeCompression, mUltracodeEccLevel, mECIMode, mEncodingMode);
                    encodedData = encoder.EncodeData();
                    barcodeText = encoder.BarcodeText;
                    break;
            }

            isValidFlag = (encodedData != null);
        }

        /// <summary>
        /// Tests if the current symbol is a member of the EAN/UPC family.
        /// </summary>
        /// <returns>true if is</returns>
        public bool IsEanUpc()
        {
            if (symbolId == Symbology.EAN13 || symbolId == Symbology.EAN8 ||
                symbolId == Symbology.UPCA || symbolId == Symbology.UPCE ||
                symbolId == Symbology.ISBN)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Tests if the current symbol is a member of GS1 Databar family.
        /// </summary>
        /// <returns>true if is</returns>
        public bool IsGS1Databar()
        {
            if (symbolId == Symbology.DatabarExpanded || symbolId == Symbology.DatabarExpandedStacked ||
                symbolId == Symbology.DatabarOmni || symbolId == Symbology.DatabarStacked ||
                symbolId == Symbology.DatabarOmniStacked || symbolId == Symbology.DatabarTruncated ||
                symbolId == Symbology.DatabarLimited)
                return true;

            else
                return false;

        }

        /// <summary>
        /// Calculate the size of the barcode symbol including any bearers and binders.
        /// </summary>
        /// <returns>symbol dimensions</returns>
        private SKSize GetSymbolOnlySize()
        {
            // Symbol dimensions without text.
            int rowWidth;
            int maxWidth = 0;
            float quietZone = 0.0f;
            SKSize symbolSize = new SKSize(0.0f, 0.0f);
            float elementWidth = mXDimension * mMultiplier;

            firstBarXOffset = 0.0f;
            // Maxicode is a fixed dimension symbol.
            if (symbolId == Symbology.MaxiCode)
            {
                symbolSize.Width = 28.16f * mMultiplier;
                symbolSize.Height = 26.86f * mMultiplier;
            }
            else
            {
                IEnumerator<SymbolData> enumerator = encodedData.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    linearShiftCount = 0;
                    rowWidth = 0;
                    SymbolData symbolData = enumerator.Current;
                    byte[] rowData = symbolData.GetRowData();
                    // Calculate the linear shift for a composite symbol by counting the leading space elements.
                    if (isCompositeSymbol)
                    {
                        int j = 0;
                        while (rowData[j] == 0)
                        {
                            linearShiftCount++;
                            j++;
                        }
                    }

                    for (int i = 0; i < symbolData.RowCount; i++)
                    {
                        if (isEanUpc && rowData[i] > 1) // Skip ean/upc bar size markers.
                            continue;

                        rowWidth++;
                    }

                    maxWidth = Math.Max(maxWidth, rowWidth);
                    // Use the height if specified, else use the property value. 
                    if (symbolData.RowHeight == 0.0f)
                        symbolSize.Height += mBarcodeHeight;
                    else
                        symbolSize.Height += symbolData.RowHeight * elementWidth;
                }

                symbolSize.Width = maxWidth * elementWidth;

                // Make allowance for ITF14 bearer bars.
                if (symbolId == Symbology.ITF14 && mITF14BearerStyle != ITF14BearerStyle.None)
                {
                    // Bearer and quite zone dimensions.
                    quietZone = 10.16f * elementWidth;
                    bearerWidth = 4.7f * elementWidth;
                    symbolSize.Width += (quietZone * 2);
                    firstBarXOffset = quietZone;
                    if (mITF14BearerStyle == ITF14BearerStyle.Rectangle)
                    {
                        symbolSize.Width += bearerWidth;
                        firstBarXOffset = quietZone + (bearerWidth / 2);
                    }

                    symbolSize.Height += bearerWidth;
                }

                // Make allowances for CodablockF top and bottom binders.
                if (symbolId == Symbology.CodablockF)
                    symbolSize.Height += elementWidth;

                if (symbolId == Symbology.Code16K || symbolId == Symbology.Code49)
                {
                    symbolSize.Height += elementWidth;
                    quietZone = 10.16f * elementWidth;
                    firstBarXOffset = quietZone;
                    symbolSize.Width += quietZone + (2.0f * elementWidth);
                }
            }

            return symbolSize;
        }

        /// <summary>
        /// Gets the overall size of the symbol including any text.
        /// </summary>
        private void GetTotalSymbolSize()
        {
            leftCharacterWidth = 0.0f;
            humanReadableText = String.Empty;
            float elementWidth = mXDimension * mMultiplier;

            SKSize symbolSize = GetSymbolOnlySize();
            symbolWidth = symbolSize.Width;
            symbolHeight = symbolSize.Height;
            if ((!String.IsNullOrEmpty(barcodeText) && mTextVisible) || isEanUpc)
            {
                if (isEanUpc)
                {
                    GetEanUpcTextSize();
                    symbolHeight += currentTextSize.Height;
                    if (leftCharacterWidth > 0)
                        firstBarXOffset = leftCharacterWidth - (linearShiftCount * elementWidth);

                    if (!String.IsNullOrEmpty(leftHandCharacter))
                        symbolWidth += leftCharacterWidth - (linearShiftCount * elementWidth);

                    symbolWidth += leftCharacterWidth;
                }
                else
                {
                    // Get the text size for a linear 1D symbol.
                    GetTextSize();
                    symbolHeight += mTextMargin + currentTextSize.Height;
                    // Text is wider than the symbol.
                    if (currentTextSize.Width > symbolWidth)
                    {
                        firstBarXOffset = (currentTextSize.Width - symbolWidth) / 2;
                        symbolWidth = currentTextSize.Width;
                    }
                }
            }
        }

        /// <summary>
        /// Optimize the font size to the linear symbol.
        /// </summary>
        void GetTextSize()
        {
            textFont = mFont;
            float fontSize = mFont.Size;

            // Build the human readable text.
            humanReadableText = barcodeText;
            if (!String.IsNullOrEmpty(checkDigitText) && mShowCheckDigit)
                humanReadableText += checkDigitText;

            if (symbolId == Symbology.Code39 || symbolId == Symbology.Code39Extended)
                humanReadableText = "*" + humanReadableText + "*";

            // Optimize the text size using SkiaSharp
            using (var paint = new SKPaint { Typeface = mFont.Typeface, TextSize = fontSize, IsAntialias = true })
            {
                currentTextSize = new SKSize(paint.MeasureText(humanReadableText), paint.FontMetrics.Descent - paint.FontMetrics.Ascent);
                if (currentTextSize.Width > symbolWidth)
                {
                    do
                    {
                        fontSize -= 1.0f;
                        if (fontSize <= 4.0f)
                            break;
                        textFont = new SKFont(mFont.Typeface, fontSize);
                        paint.TextSize = fontSize;
                        currentTextSize = new SKSize(paint.MeasureText(humanReadableText), paint.FontMetrics.Descent - paint.FontMetrics.Ascent);
                    } while (currentTextSize.Width >= symbolWidth);
                }
            }
        }

        /// <summary>
        /// Optimize the text font size that will fit between the guard bars.
        /// </summary>
        private void GetEanUpcTextSize()
        {
            float fontSize = 1 * mMultiplier;
            float maxTextWidth = 7 * mXDimension * mMultiplier * leftHandText.Length;
            SKSize textSize;
            using (var paint = new SKPaint { Typeface = mFont.Typeface, TextSize = fontSize, IsAntialias = true })
            {
                do
                {
                    fontSize += 0.5f;
                    textFont = new SKFont(mFont.Typeface, fontSize);
                    paint.TextSize = fontSize;
                    textSize = new SKSize(paint.MeasureText(leftHandText), paint.FontMetrics.Descent - paint.FontMetrics.Ascent);
                } while (textSize.Width < maxTextWidth);

                fontSize -= 1.0f;
                textFont = new SKFont(mFont.Typeface, fontSize);
                paint.TextSize = fontSize;
                textSize = new SKSize(paint.MeasureText(leftHandText), paint.FontMetrics.Descent - paint.FontMetrics.Ascent);
                if (!string.IsNullOrEmpty(leftHandCharacter))
                    leftCharacterWidth = paint.MeasureText(leftHandCharacter);

                currentTextSize = textSize;
            }
        }
    }
}
