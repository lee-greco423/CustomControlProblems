using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace WindowsFormsControlLibrary1
{
    [TypeConverter(typeof(ColorPairConverter))]
    public class ColorPair
    {
        static ColorConverter colorConverter = new ColorConverter();

        #region constructors
        public ColorPair() { }

        public ColorPair(Color ColorA, Color ColorB)
        {
            colorA = ColorA;
            colorB = ColorB;
        }

        public ColorPair(string valueString) : this(valueString.Split(';')[0], valueString.Split(';')[1]) { }

        public ColorPair(string colorStringA, string colorStringB)
        {
            ColorConverter converter = new ColorConverter();
            this.colorA = (Color)converter.ConvertFromString(colorStringA);
            this.colorB = (Color)converter.ConvertFromString(colorStringB);
        }
        #endregion

        private Color colorA = Color.Blue;
        public Color ColorA
        {
            get { return this.colorA; }
            set { this.colorA = value; }
        }

        private Color colorB = Color.White;
        public Color ColorB
        {
            get { return this.colorB; }
            set { this.colorB = value; }
        }

        public override string ToString()
        {
            return string.Format("{0};{1}", colorConverter.ConvertToString(colorA), colorConverter.ConvertToString(colorB));
        }

        public override bool Equals(object obj)
        {
            return (ToString() ?? "") == (((ColorPair)obj).ToString() ?? "");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public partial class ColorPairConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (ReferenceEquals(sourceType, typeof(string)))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return new ColorPair((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(String))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                ColorPair i = (ColorPair)value;
                return i.ToString();
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                var ci = typeof(ColorPair).GetConstructor(new Type[] { typeof(string) });
                ColorPair i = (ColorPair)value;
                return new InstanceDescriptor(ci, new object[] { i.ToString() });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues is null)
            {
                throw new ArgumentNullException(nameof(propertyValues));
            }
            Color colorA = (Color)propertyValues[nameof(ColorPair.ColorA)];
            Color colorB = (Color)propertyValues[nameof(ColorPair.ColorB)];
            return new ColorPair(colorA, colorB);
        }
    }

    public class ColorPairEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            // Erase the area.
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            ColorPair colorPair;
            if (e.Context == null)
            {
                colorPair = new ColorPair();
            }
            else
            {
                colorPair = (ColorPair)e.Value;
            }
            // Draw the example.
            using (var br = new LinearGradientBrush(e.Bounds, colorPair.ColorA, colorPair.ColorB, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(br, e.Bounds);
            }
            using (var border_pen = new Pen(Color.Black, 1f))
            {
                e.Graphics.DrawRectangle(border_pen, 1, 1, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }
        }
    }
}
