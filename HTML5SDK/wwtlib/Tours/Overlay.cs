﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Html;
using System.Xml;
using System.Html.Services;
using System.Html.Media.Graphics;

namespace wwtlib
{
    public enum OverlayAnchor { Sky = 0, Screen = 1 };
    public enum AudioType { Music = 0, Voice = 1 };

    public abstract class Overlay
    {
        public const string ClipboardFormat = "WorldWideTelescope.Overlay";

        protected bool isDesignTimeOnly = false;
        string name;
        public static int NextId = 11231;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public const double RC = 3.1415927 / 180;
        //todo no Guid in jscript.. we should only be reading and not creating so we are ok for now
        public string Id = (NextId++).ToString();//Guid.NewGuid().ToString();

        TourStop owner = null;

        public TourStop Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private string url = "";

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string linkID = "";

        public string LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

        virtual public void Play()
        {
        }


        virtual public void Pause()
        {
        }


        virtual public void Stop()
        {
        }


        virtual public void Seek(double time)
        {
        }

        virtual public void Draw3D(RenderContext renderContext, bool designTime)
        {

        }

        virtual public void CleanUp()
        {
            if (texture != null)
            {
                texture = null;
            }
        }

        virtual public void InitializeTexture()
        {
        }

        virtual public void CleanUpGeometry()
        {
            currentRotation = 0;
        }

        virtual public void InitiaizeGeometry()
        {

        }

        virtual public void UpdateRotation()
        {


        }
        // Animation Support
        bool animate;

        public bool Animate
        {
            get { return animate; }
            set
            {
                if (animate != value)
                {
                    animate = value;

                    if (animate)
                    {
                        endX = x;
                        endY = y;
                        endRotationAngle = rotationAngle;
                        endColor = color;
                        endWidth = width;
                        endHeight = height;
                        CleanUpGeometry();
                    }
                    else
                    {
                        endX = x = X;
                        endY = y = Y;
                        endRotationAngle = rotationAngle = RotationAngle;
                        endColor = color = Color;
                        endWidth = width = Width;
                        endHeight = height = Height;
                        CleanUpGeometry();
                        tweenFactor = 0;
                    }
                }
            }
        }
        double tweenFactor = 0;

        public double TweenFactor
        {
            get
            {
                return tweenFactor;
            }
            set
            {
                if (!animate)
                {
                    tweenFactor = 0;
                }
                else
                {
                    if (tweenFactor != value)
                    {
                        tweenFactor = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        double endX;
        double endY;
        double endOpacity;
        Color endColor = new Color();
        double endWidth;
        double endHeight;
        double endRotationAngle;



        // End Animation Support


        OverlayAnchor anchor;

        public OverlayAnchor Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }

        public Vector2d Position
        {
            get
            {
                return Vector2d.Create(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        private double x;



        public double X
        {
            get { return (x * (1 - tweenFactor)) + (endX * tweenFactor); }
            set
            {
                if (tweenFactor < .5f)
                {
                    if (x != value)
                    {
                        x = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endX != value)
                    {
                        endX = value;
                        CleanUpGeometry();
                    }
                }
            }
        }
        private double y;


        public double Y
        {
            get { return (y * (1 - tweenFactor)) + (endY * tweenFactor); }
            set
            {
                if (tweenFactor < .5f)
                {
                    if (y != value)
                    {
                        y = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endY != value)
                    {
                        endY = value;
                        CleanUpGeometry();
                    }
                }
            }
        }
        private double width;


        public double Width
        {
            get { return (width * (1 - tweenFactor)) + (endWidth * tweenFactor); }
            set
            {
                if (value < 5 && value != 0)
                {
                    value = 5;
                }

                if (tweenFactor < .5f)
                {
                    if (width != value)
                    {
                        width = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endWidth != value)
                    {
                        endWidth = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        private double height;


        public double Height
        {
            get { return (height * (1 - tweenFactor)) + (endHeight * tweenFactor); }
            set
            {
                if (value < 5 && value != 0)
                {
                    value = 5;
                }

                if (tweenFactor < .5f)
                {
                    if (height != value)
                    {
                        height = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endHeight != value)
                    {
                        endHeight = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        private Color color = Colors.White;


        public virtual Color Color
        {
            get
            {
                int red = (int)(((double)color.R * (1f - tweenFactor)) + ((double)endColor.R * tweenFactor));
                int green = (int)(((double)color.G * (1f - tweenFactor)) + ((double)endColor.G * tweenFactor));
                int blue = (int)(((double)color.B * (1f - tweenFactor)) + ((double)endColor.B * tweenFactor));
                int alpha = (int)(((double)color.A * (1f - tweenFactor)) + ((double)endColor.A * tweenFactor));
                return Color.FromArgb((byte)Math.Max(0, Math.Min(255, alpha)), (byte)Math.Max(0, Math.Min(255, red)), (byte)Math.Max(0, Math.Min(255, green)), (byte)Math.Max(0, Math.Min(255, blue)));
            }
            set
            {
                if (tweenFactor < .5f)
                {
                    if (color != value)
                    {
                        color = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endColor != value)
                    {
                        endColor = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        private double opacity = .5f;


        public double Opacity
        {
            get
            {
                return (double)Color.A / 255.0f;
            }
            set
            {
                Color col = Color;
                this.Color = Color.FromArgb((byte)Math.Min(255, (int)(value * 255f)), col.R, col.G, col.B);
                opacity = value;
            }
        }

        double rotationAngle = 0;
        protected double currentRotation = 0;


        public double RotationAngle
        {
            get { return (rotationAngle * (1 - tweenFactor)) + (endRotationAngle * tweenFactor); }
            set
            {
                if (tweenFactor < .5f)
                {
                    if (rotationAngle != value)
                    {
                        rotationAngle = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endRotationAngle != value)
                    {
                        endRotationAngle = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        protected ImageElement texture = null;

        virtual public bool HitTest(Vector2d pntTest)
        {
            //todo this needs to be translated to script#

            //Matrix3d mat = new Matrix3d();
            //mat.RotateAt(new Quaternion(new Vector3D(0,0,1), -RotationAngle), new Point3D(X , Y , 0 ));

            //Point3D tempPoint = new Point3D(pntTest.X, pntTest.Y, 0);

            //tempPoint  = mat.Transform(tempPoint);

            //Rect rect = new Rect((X-(Width/2)), (Y-(Height/2)), Width, Height);
            //if (rect.Contains(new Point(tempPoint.X,tempPoint.Y)))
            //{
            //    return true;
            //}
            return false;
        }


        Rectangle bounds;

        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }

        private InterpolationType interpolationType = InterpolationType.DefaultV;

        public InterpolationType InterpolationType
        {
            get { return interpolationType; }
            set { interpolationType = value; }
        }

        internal static Overlay FromXml(TourStop owner, XmlNode overlay)
        {
            if (overlay.Attributes == null)
            {
                return null;
            }

            if (overlay.Attributes.GetNamedItem("Type") == null)
            {
                return null;
            }
            string overlayClassName = overlay.Attributes.GetNamedItem("Type").Value.ToString();

            //Type overLayType = Type.GetType(overlayClassName.Replace("TerraViewer.",""));
            string overLayType = overlayClassName.Replace("TerraViewer.", "");
            Overlay newOverlay = null;

            //Overlay newOverlay = (Overlay)System.Activator.CreateInstance(overLayType);
            switch (overLayType)
            {
                case "AudioOverlay":
                    newOverlay = new AudioOverlay();
                    break;
                case "BitmapOverlay":
                    newOverlay = new BitmapOverlay();
                    break;
                case "FlipBookOverlay":
                    newOverlay = new FlipbookOverlay();
                    break;
                case "ShapeOverlay":
                    newOverlay = new ShapeOverlay();
                    break;
                case "TextOverlay":
                    newOverlay = new TextOverlay();
                    break;
                default:
                    return null;
            }

            newOverlay.owner = owner;
            newOverlay.InitOverlayFromXml(overlay);
            return newOverlay;
        }

        private void InitOverlayFromXml(XmlNode node)
        {
            Id = node.Attributes.GetNamedItem("Id").Value;
            Name = node.Attributes.GetNamedItem("Name").Value;
            x = double.Parse(node.Attributes.GetNamedItem("X").Value);
            y = double.Parse(node.Attributes.GetNamedItem("Y").Value);
            width = double.Parse(node.Attributes.GetNamedItem("Width").Value);
            height = double.Parse(node.Attributes.GetNamedItem("Height").Value);
            rotationAngle = double.Parse(node.Attributes.GetNamedItem("Rotation").Value);
            color = Color.Load(node.Attributes.GetNamedItem("Color").Value);
            if (node.Attributes.GetNamedItem("Url") != null)
            {
                Url = node.Attributes.GetNamedItem("Url").Value;
            }

            if (node.Attributes.GetNamedItem("LinkID") != null)
            {
                LinkID = node.Attributes.GetNamedItem("LinkID").Value;
            }

            if (node.Attributes.GetNamedItem("Animate") != null)
            {
                animate = bool.Parse(node.Attributes.GetNamedItem("Animate").Value);
                if (animate)
                {
                    endX = double.Parse(node.Attributes.GetNamedItem("EndX").Value);
                    endY = double.Parse(node.Attributes.GetNamedItem("EndY").Value);
                    endColor = Color.Load(node.Attributes.GetNamedItem("EndColor").Value);
                    endWidth = double.Parse(node.Attributes.GetNamedItem("EndWidth").Value);
                    endHeight = double.Parse(node.Attributes.GetNamedItem("EndHeight").Value);
                    endRotationAngle = double.Parse(node.Attributes.GetNamedItem("EndRotation").Value);
                    if (node.Attributes.GetNamedItem("InterpolationType") != null)
                    {
                        switch (node.Attributes.GetNamedItem("InterpolationType").Value)
                        {
                            case "Linear":
                                InterpolationType = InterpolationType.Linear;
                                break;
                            case "EaseIn":
                                InterpolationType = InterpolationType.EaseIn;
                                break;
                            case "EaseOut":
                                InterpolationType = InterpolationType.EaseOut;
                                break;
                            case "EaseInOut":
                                InterpolationType = InterpolationType.EaseInOut;
                                break;
                            case "Exponential":
                                InterpolationType = InterpolationType.Exponential;
                                break;
                            case "Default":
                                InterpolationType = InterpolationType.DefaultV;
                                break;
                            default:
                                break;
                        }
                    } 
                }
            }

            InitializeFromXml(node);
        }

        public virtual void InitializeFromXml(XmlNode node)
        {

        }

        public override string ToString()
        {
            return this.Name;
        }

    }
    public class BitmapOverlay : Overlay
    {
        string filename;

        public BitmapOverlay()
        {

        }

        //public static BitmapOverlay(RenderContext renderContext, TourStop owner, string filename)
        //{
        //    this.Owner = owner;
        //    this.filename = Guid.NewGuid().ToString() + ".png";

        //    this.Name = filename.Substr(filename.LastIndexOf('\\'));

        //    X = 0;
        //    Y = 0;
        //}


        public static BitmapOverlay Create(RenderContext renderContext, TourStop owner, ImageElement image)
        {
            BitmapOverlay temp = new BitmapOverlay();

            temp.Owner = owner;
            // to make directory and guid filename in tour temp dir.
            temp.filename = (NextId++).ToString() + ".png";

            temp.Name = owner.GetNextDefaultName("Image");
            temp.X = 0;
            temp.Y = 0;

            return temp;
        }

        public BitmapOverlay Copy(TourStop owner)
        {
            BitmapOverlay newBmpOverlay = new BitmapOverlay();
            newBmpOverlay.Owner = owner;
            newBmpOverlay.filename = this.filename;
            newBmpOverlay.X = this.X;
            newBmpOverlay.Y = this.Y;
            newBmpOverlay.Width = this.Width;
            newBmpOverlay.Height = this.Height;
            newBmpOverlay.Color = this.Color;
            newBmpOverlay.Opacity = this.Opacity;
            newBmpOverlay.RotationAngle = this.RotationAngle;
            newBmpOverlay.Name = this.Name + " - Copy";

            return newBmpOverlay;
        }

        public override void CleanUp()
        {
            texture = null;
        }

        bool textureReady = false;
        public override void InitializeTexture()
        {
            try
            {
                texture = Owner.Owner.GetCachedTexture(filename, delegate { textureReady = true; });

                if (Width == 0 && Height == 0)
                {
                    //Width = texture.Width;
                    //Height = texture.Height;

                }
            }
            catch
            {

            }
        }
        private ImageElement imageBrush;
        public override void Draw3D(RenderContext renderContext, bool designTime)
        {
            if (texture == null)
            {
                InitializeTexture();
            }

            if (!textureReady)
            {
                return;
            }
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();

            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle*RC);
            ctx.Alpha = Opacity;
            ctx.DrawImage(texture, - Width / 2, - Height / 2, Width, Height);
            ctx.Restore();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode bitmap = Util.SelectSingleNode(node, "Bitmap");
            filename = bitmap.Attributes.GetNamedItem("Filename").Value;
        }
    }

    public class TextOverlay : Overlay
    {
        public TextObject TextObject;
        public override Color Color
        {
            get
            {
                return base.Color;
                //return TextObject.ForgroundColor;
            }
            set
            {
                if (TextObject.ForgroundColor != value)
                {
                    TextObject.ForgroundColor = value;
                    base.Color = value;
                    CleanUp();
                }
            }
        }

        public TextOverlay()
        {
        }

        //public static TextOverlay(Canvas canvas, TextObject textObject)
        //{
        //    this.canvas = canvas;
        //    this.TextObject = textObject;
        //    this.Name = textObject.Text.Split(new char[] { '\r', '\n' })[0];
        //    X = 0;
        //    Y = 0;

        //}


        public override void Draw3D(RenderContext renderContext, bool designTime)
        {
            //TextBlock textBlock = new TextBlock();
            //textBlock.Width = this.Width;
            //textBlock.Height = this.Height;
            //textBlock.Foreground = new SolidColorBrush(TextObject.ForgroundColor);
            //textBlock.Text = TextObject.Text;
            //textBlock.FontWeight = TextObject.Bold ? FontWeights.Bold : FontWeights.Normal;
            //textBlock.FontSize = TextObject.FontSize * 1.2;
            //textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            //TranslateTransform tt = new TranslateTransform();
            //tt.X = this.X - (Width / 2);
            //tt.Y = this.Y - (Height / 2);
            //textBlock.RenderTransform = tt;
            //canvas.Children.Add(textBlock);
            //textBlock.Opacity = this.Opacity;

            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();

            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle*RC);
            ctx.Alpha = Opacity;
            ctx.FillStyle = TextObject.ForgroundColor.ToString();
            ctx.Font = (TextObject.Italic ? "italic" : "normal") + " " + (TextObject.Bold ? "bold" : "normal") + " " + Math.Round(TextObject.FontSize * 1.2).ToString() + "px " + TextObject.FontName;
            ctx.TextBaseline = TextBaseline.Top;

            String text = TextObject.Text;

            if (text.IndexOf("{$") >  -1)
            {
                if (text.IndexOf("{$DATE}") > -1)
                {
                    string date = String.Format("{0:yyyy/MM/dd}",SpaceTimeController.Now);
                    text = text.Replace("{$DATE}", date);
                }

                if (text.IndexOf("{$TIME}") > -1)
                {
                    string time =  String.Format("{0:HH:mm:ss}", SpaceTimeController.Now);
                    text = text.Replace("{$TIME}", time);
                }


              //  text = text.Replace("{$DIST}", UiTools.FormatDistance(WWTControl.Singleton.SolarSystemCameraDistance));
                text = text.Replace("{$LAT}", Coordinates.FormatDMS(WWTControl.Singleton.RenderContext.ViewCamera.Lat));
                text = text.Replace("{$LNG}", Coordinates.FormatDMS(WWTControl.Singleton.RenderContext.ViewCamera.Lat));
                text = text.Replace("{$RA}", Coordinates.FormatDMS(WWTControl.Singleton.RenderContext.ViewCamera.RA));
                text = text.Replace("{$DEC}", Coordinates.FormatDMS(WWTControl.Singleton.RenderContext.ViewCamera.Dec));
                text = text.Replace("{$FOV}", Coordinates.FormatDMS(WWTControl.Singleton.RenderContext.FovAngle));
            }





            string[] lines = text.Split("\n");

            double baseline =  - (Height / 2);
            double lineSpace = TextObject.FontSize * 1.7;

            foreach (string line in lines)
            {
                List<string> parts = Util.GetWrappedText(ctx, line, Width);
                foreach (string part in parts)
                {
                    ctx.FillText(part, -Width / 2, baseline);
                    baseline += lineSpace;
                }
            }

            ctx.Restore();
        }

        public override void InitializeTexture()
        {
            //System.Drawing.Font font = TextObject.Font;
            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Near;

            //Bitmap bmp = new Bitmap(20, 20);
            //Graphics g = Graphics.FromImage(bmp);
            //SizeF size = g.MeasureString(TextObject.Text, font);
            //g.Dispose();
            //bmp.Dispose();

            //double border =0;

            //switch (TextObject.BorderStyle)
            //{
            //    case TextBorderStyle.None:
            //    case TextBorderStyle.Tight:
            //        border = 0;
            //        break;
            //    case TextBorderStyle.Small:
            //        border = 10;
            //        break;
            //    case TextBorderStyle.Medium:
            //        border = 15;
            //        break;
            //    case TextBorderStyle.Large:
            //        border = 20;
            //        break;
            //    default:
            //        break;
            //}
            //if (size.Width == 0 || size.Height == 0)
            //{
            //    size = new SizeF(1, 1);
            //}
            //bmp = new Bitmap((int)(size.Width + (border * 2)), (int)(size.Height + (border * 2)));
            //g = Graphics.FromImage(bmp);
            //if (TextObject.BorderStyle != TextBorderStyle.None)
            //{
            //    g.Clear(TextObject.BackgroundColor);
            //}

            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            ////g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //Brush textBrush = new SolidBrush(TextObject.ForgroundColor);

            //g.DrawString(TextObject.Text, font, textBrush, border, border, sf);
            //textBrush.Dispose();
            //g.Dispose();
            //texture = UiTools.LoadTextureFromBmp(device, bmp);
            //bmp.Dispose();
            //font.Dispose();
            //if (Width == 0 && Height == 0)
            //{
            //    Width = size.Width + (border * 2);
            //    Height = size.Height + (border * 2);
            //}
        }



        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode text = Util.SelectSingleNode(node, "Text");

            TextObject = TextObject.FromXml(Util.SelectSingleNode(text, "TextObject"));

        }


        override public void InitiaizeGeometry()
        {

        }
    }

    public enum ShapeType { Circle = 0, Rectagle = 1, Star = 2, Donut = 3, Arrow = 4, Line = 5, OpenRectagle = 6 };
    public class ShapeOverlay : Overlay
    {
        ShapeType shapeType = ShapeType.Rectagle;

        public ShapeOverlay()
        {
        }


        public ShapeType ShapeType
        {
            get { return shapeType; }
            set
            {
                shapeType = value;
                CleanUpGeometry();
            }
        }

        //public ShapeOverlay(RenderContext renderContext, TourStop owner, ShapeType shapeType)
        //{
        //    ShapeType = shapeType;
        //    this.Owner = owner;
        //    this.Name = owner.GetNextDefaultName(shapeType.ToString());
        //}

        public override void Draw3D(RenderContext renderContext, bool designTime)
        {

            switch (shapeType)
            {
                case ShapeType.Circle:
                    CreateCircleGeometry(renderContext);
                    break;
                case ShapeType.Rectagle:
                    CreateRectGeometry(renderContext);
                    break;
                case ShapeType.OpenRectagle:
                    CreateOpenRectGeometry(renderContext);
                    break;
                case ShapeType.Star:
                    CreateStarGeometry(renderContext);
                    break;
                case ShapeType.Donut:
                    CreateDonutGeometry(renderContext);
                    break;
                case ShapeType.Arrow:
                    CreateArrowGeometry(renderContext);
                    break;
                case ShapeType.Line:
                    CreateLineGeometry(renderContext);
                    break;
                default:
                    break;
            }

        }
        public override void InitiaizeGeometry()
        {

        }
        private void CreateLineGeometry(RenderContext renderContext)
        {

            //todo this needs to be Dashed rounded lines..

            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            double radius = Width / 2;
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle*RC);

            ctx.MoveTo(-radius, 0);
            ctx.LineTo(radius, 0);
            ctx.LineWidth = 9;
            ctx.StrokeStyle = Color.ToString();
            ctx.Alpha = Opacity;
            ctx.Stroke();
            ctx.Restore();


        }
        private void CreateOpenRectGeometry(RenderContext renderContext)
        {
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle*RC);
 
            ctx.BeginPath();
            ctx.MoveTo(-Width / 2, -Height / 2);
            ctx.LineTo(Width / 2, -Height / 2);
            ctx.LineTo(Width / 2, Height / 2);
            ctx.LineTo(-Width / 2, Height / 2);
            ctx.ClosePath();
            ctx.LineWidth = 9;
            ctx.StrokeStyle = Color.ToString();

           ctx.Alpha = Opacity;
            ctx.Stroke();
            ctx.Restore();
        }

        private void CreateRectGeometry(RenderContext renderContext)
        {
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle * RC);

            ctx.BeginPath();
            ctx.MoveTo(-Width / 2, -Height / 2);
            ctx.LineTo(Width / 2, -Height / 2);
            ctx.LineTo(Width / 2, +Height / 2);
            ctx.LineTo(-Width / 2, +Height / 2);
            ctx.ClosePath();
            ctx.LineWidth = 0;
            ctx.FillStyle = Color.ToString();

            ctx.Alpha = Opacity;
            ctx.Fill();
            ctx.Restore();
        }
        private void CreateStarGeometry(RenderContext renderContext)
        {

            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle * RC);
            ctx.BeginPath();

            double centerX = 0;
            double centerY = 0;
            double radius = Width / 2;

            double radiansPerSegment = ((double)Math.PI * 2) / 5;

            bool first = true;

            for (int i = 0; i < 5; i++)
            {
                double rads = i * radiansPerSegment - (Math.PI / 2);
                if (first)
                {
                    first = false;
                    ctx.MoveTo(centerX + Math.Cos(rads) * (Width / 2), centerY + Math.Sin(rads) * (Height / 2));
                }
                else
                {
                    ctx.LineTo(centerX + Math.Cos(rads) * (Width / 2), centerY + Math.Sin(rads) * (Height / 2));
                }

                double rads2 = i * radiansPerSegment + (radiansPerSegment / 2) - (Math.PI / 2);
                ctx.LineTo(centerX + Math.Cos(rads2) * (Width / 5.3), centerY + Math.Sin(rads2) * (Height / 5.3));

            }

            ctx.ClosePath();
            ctx.LineWidth = 0;
            ctx.FillStyle = Color.ToString();


            ctx.Alpha = Opacity;
            ctx.Fill();
            ctx.Restore();
        }
        private void CreateArrowGeometry(RenderContext renderContext)
        {
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle * RC);
 
            ctx.BeginPath();
            ctx.MoveTo((-(Width / 2)), (-(Height / 4)));
            ctx.LineTo(((Width / 4)), (-(Height / 4)));
            ctx.LineTo(((Width / 4)), (-(Height / 2)));
            ctx.LineTo(((Width / 2)), 0);
            ctx.LineTo(((Width / 4)), ((Height / 2)));
            ctx.LineTo(((Width / 4)), ((Height / 4)));
            ctx.LineTo((-(Width / 2)), ((Height / 4)));
            ctx.ClosePath();
            ctx.LineWidth = 0;
            ctx.FillStyle = Color.ToString();

           ctx.Alpha = Opacity;
            ctx.Fill();
            ctx.Restore();
        }

        private void CreateDonutGeometry(RenderContext renderContext)
        {
            //todo move to dashed lines in future
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Translate(X, Y);
            ctx.Scale(1, Height / Width);
            ctx.Rotate(RotationAngle * RC);
            ctx.BeginPath();
            ctx.Arc(0, 0, Width / 2, 0, Math.PI * 2, false);

            ctx.ClosePath();
            ctx.LineWidth = 9;
            ctx.StrokeStyle = Color.ToString();
            ctx.Alpha = Opacity;
            ctx.Stroke();
            ctx.Restore();


        }

        private void CreateCircleGeometry(RenderContext renderContext)
        {
            CanvasContext2D ctx = renderContext.Device;
            ctx.Save();
            ctx.Scale(1, Width / Height);
            ctx.Translate(X, Y);
            ctx.Rotate(RotationAngle * RC);
            ctx.BeginPath();
            ctx.Arc(0, 0, Width, 0, Math.PI * 2, false);
            ctx.ClosePath();
            ctx.LineWidth = 0;
            ctx.FillStyle = Color.ToString();
            ctx.Alpha = Opacity;
            ctx.Fill();
            ctx.Restore();
        }

        public override void InitializeTexture()
        {

        }

        public override void CleanUpGeometry()
        {
            base.CleanUpGeometry();
            CleanUp();
        }



        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode shape = Util.SelectSingleNode(node, "Shape");
            //shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shape.Attributes.GetNamedItem("ShapeType").Value, true);

            switch (shape.Attributes.GetNamedItem("ShapeType").Value)
            {

                case "Circle":
                    shapeType = ShapeType.Circle;
                    break;
                case "Rectagle":
                    shapeType = ShapeType.Rectagle;
                    break;
                case "Star":
                    shapeType = ShapeType.Star;
                    break;
                case "Donut":
                    shapeType = ShapeType.Donut;
                    break;
                case "Arrow":
                    shapeType = ShapeType.Arrow;
                    break;
                case "Line":
                    shapeType = ShapeType.Line;
                    break;
                default:
                case "OpenRectagle":
                    shapeType = ShapeType.OpenRectagle;
                    break;

            }
        }
    }
    public class AudioOverlay : Overlay
    {
        string filename;
        AudioElement audio = null;
        int volume = 100;

        bool mute = false;

        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                Volume = Volume;
            }
        }

        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (audio != null)
                {
                    audio.Volume = this.mute ? 0 : (float)(volume / 100.0);
                }
            }
        }

        public AudioOverlay()
        {
            isDesignTimeOnly = true;

        }

        //void audio_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        //{
        //    audio.Stop();
        //}


        public override void Play()
        {
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null && audioReady)
            {
                audio.Play();
                Volume = Volume;
            }
        }


        public override void Pause()
        {
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null && audioReady)
            {
                audio.Pause();
            }
        }


        public override void Stop()
        {
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null && audioReady)
            {
                audio.Pause();
            }
        }

        double position = 0;
        public override void Seek(double time)
        {
            position = time;
            if (audio == null)
            {
                InitializeTexture();
            }
            //todo double check time

            if (audioReady)
            {
                if (audio.Duration < time)
                {
                    audio.Pause();
                }
                else
                {
                    audio.CurrentTime = position;
                }
            }
        }
        bool audioReady = false;
        //public AudioOverlay(RenderContext renderContext, TourStop owner, string filename)
        //{
        //    isDesignTimeOnly = true;
        //    X = 0;
        //    Y = 0;
        //    this.filename = Guid.NewGuid().ToString() + filename.Substr(filename.LastIndexOf("."));
        //    this.Owner = owner;
        //    this.Name = owner.GetNextDefaultName("Audio");
        //    // File.Copy(filename, Owner.Owner.WorkingDirectory + this.filename);
        //}

        public override void InitializeTexture()
        {
            if (audio == null)
            {
                audio = (AudioElement)Document.CreateElement("audio");
                //audio.AutoPlay = true;
                //audio.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(audio_MediaFailed);
                //audio.MediaOpened += new RoutedEventHandler(audio_MediaOpened);
                //Viewer.MasterView.audio.Children.Add(audio);
                audio.Src = Owner.Owner.GetFileStream(this.filename);
                audio.AddEventListener("canplaythrough", delegate
                {
                    audioReady = true;
                    audio_MediaOpened();
                    audio.Play();
                }, false);


            }

        }

        public override void CleanUp()
        {
            base.CleanUp();

            if (audio != null)
            {
                audio.Pause();
                audio.Src = null;
                audio = null;
            }

        }

        void audio_MediaOpened()
        {
            audio.CurrentTime = position;

            audio.Volume = this.mute ? 0 : (float)(volume / 100.0);

        }

        AudioType trackType = AudioType.Music;

        public AudioType TrackType
        {
            get { return trackType; }
            set { trackType = value; }
        }



        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode audio = Util.SelectSingleNode(node, "Audio");
            filename = audio.Attributes.GetNamedItem("Filename").Value;
            if (audio.Attributes.GetNamedItem("Volume") != null)
            {
                volume = int.Parse(audio.Attributes.GetNamedItem("Volume").Value);
            }

            if (audio.Attributes.GetNamedItem("Mute") != null)
            {
                mute = bool.Parse(audio.Attributes.GetNamedItem("Mute").Value);
            }

            if (audio.Attributes.GetNamedItem("TrackType") != null)
            {
                switch (audio.Attributes.GetNamedItem("TrackType").Value)
                {
                    case "Music":
                        trackType = AudioType.Music;
                        break;
                    default:
                    case "Voice":
                        trackType = AudioType.Voice;
                        break;
                }
            }
        }
    }

    public enum LoopTypes { Loop = 0, UpDown = 1, Down = 2, UpDownOnce = 3, Once = 4, Begin = 5, End = 6 };

    public class FlipbookOverlay : Overlay
    {
        string filename;

        LoopTypes loopType = LoopTypes.UpDown;

        public LoopTypes LoopType
        {
            get { return loopType; }
            set { loopType = value; }
        }

        int startFrame = 0;

        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }
        List<int> framesList = new List<int>();
        string frameSequence;

        public string FrameSequence
        {
            get { return frameSequence; }
            set
            {
                if (frameSequence != value)
                {
                    frameSequence = value;
                    framesList = new List<int>();
                    if (!string.IsNullOrEmpty(frameSequence))
                    {
                        try
                        {
                            string[] parts = frameSequence.Split(",");
                            foreach (string part in parts)
                            {
                                int x = int.Parse(part.Trim());
                                framesList.Add(x);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        int frames = 1;

        public int Frames
        {
            get { return frames; }
            set
            {
                frames = value;
            }
        }

        int framesX = 8;

        public int FramesX
        {
            get { return framesX; }
            set { framesX = value; }
        }
        int framesY = 8;

        public int FramesY
        {
            get { return framesY; }
            set { framesY = value; }
        }



        public FlipbookOverlay()
        {

        }

        //public FlipbookOverlay(RenderContext renderContext, TourStop owner, string filename)
        //{
        //    this.Owner = owner;


        //    string extension = filename.Substr(filename.LastIndexOf("."));

        //    this.filename = Guid.NewGuid().ToString() + extension;

        //    this.Name = filename.Substr(filename.LastIndexOf('\\'));
        //    //File.Copy(filename, Owner.Owner.WorkingDirectory + this.filename);

        //    //Bitmap bmp = new Bitmap(Owner.Owner.WorkingDirectory + this.filename);
        //    Width = 256;
        //    Height = 256;
        //    //bmp.Dispose();
        //    //bmp = null;
        //    X = 0;
        //    Y = 0;
        //}


        //public FlipbookOverlay(RenderContext renderContext, TourStop owner, Image image)
        //{
        //    this.Owner = owner;
        //    this.canvas = canvas;
        //    // to make directory and guid filename in tour temp dir.
        //    this.filename = Guid.NewGuid().ToString() + ".png";

        //    this.Name = owner.GetNextDefaultName("Image");
        //    X = 0;
        //    Y = 0;
        //    //image.Save(Owner.Owner.WorkingDirectory + filename, ImageFormat.Png);
        //    Width = 256;
        //    Height = 256;
        //}

        public FlipbookOverlay Copy(TourStop owner)
        {
            //todo fix this
            FlipbookOverlay newFlipbookOverlay = new FlipbookOverlay();
            newFlipbookOverlay.Owner = owner;
            newFlipbookOverlay.filename = this.filename;
            newFlipbookOverlay.X = this.X;
            newFlipbookOverlay.Y = this.Y;
            newFlipbookOverlay.Width = this.Width;
            newFlipbookOverlay.Height = this.Height;
            newFlipbookOverlay.Color = this.Color;
            newFlipbookOverlay.Opacity = this.Opacity;
            newFlipbookOverlay.RotationAngle = this.RotationAngle;
            newFlipbookOverlay.Name = this.Name + " - Copy";
            newFlipbookOverlay.StartFrame = this.StartFrame;
            newFlipbookOverlay.Frames = this.Frames;
            newFlipbookOverlay.LoopType = this.LoopType;
            newFlipbookOverlay.FrameSequence = this.FrameSequence;
            newFlipbookOverlay.FramesX = this.FramesX;
            newFlipbookOverlay.FramesY = this.FramesY;

            return newFlipbookOverlay;
        }

        public override void CleanUp()
        {
            texture = null;
        }
        bool textureReady = false;
        public override void InitializeTexture()
        {
            try
            {
                bool colorKey = filename.ToLowerCase().EndsWith(".jpg");
                texture = Owner.Owner.GetCachedTexture( filename, delegate { textureReady = true; });

                //texture = UiTools.LoadTextureFromBmp(device, Owner.Owner.WorkingDirectory + filename);

                //      texture = TextureLoader.FromFile(device, Owner.Owner.WorkingDirectory + filename);

                //if (Width == 0 && Height == 0)
                //{
                //    Width = sd.Width;
                //    Height = sd.Height;

                //}
            }
            catch
            {
                //texture = UiTools.LoadTextureFromBmp(device, (Bitmap)global::TerraViewer.Properties.Resources.BadImage);
                //SurfaceDescription sd = texture.GetLevelDescription(0);

                //{
                //    Width = sd.Width;
                //    Height = sd.Height;

                //}
            }
        }



        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode flipbook = Util.SelectSingleNode(node, "Flipbook");
            filename = flipbook.Attributes.GetNamedItem("Filename").Value;
            frames = int.Parse(flipbook.Attributes.GetNamedItem("Frames").Value);
            switch (flipbook.Attributes.GetNamedItem("Loop").Value)
            {

                case "Loop":
                    loopType = LoopTypes.Loop;
                    break;
                case "UpDown":
                    loopType = LoopTypes.UpDown;
                    break;
                case "Down":
                    loopType = LoopTypes.Down;
                    break;
                case "UpDownOnce":
                    loopType = LoopTypes.UpDownOnce;
                    break;
                case "Once":
                    loopType = LoopTypes.Once;
                    break;
                case "Begin":
                    loopType = LoopTypes.Begin;
                    break;
                case "End":
                    loopType = LoopTypes.End;
                    break;
                default:
                    break;
            }


            if (flipbook.Attributes.GetNamedItem("FramesX") != null)
            {
                FramesX = int.Parse(flipbook.Attributes.GetNamedItem("FramesX").Value);
            }
            if (flipbook.Attributes.GetNamedItem("FramesY") != null)
            {
                FramesY = int.Parse(flipbook.Attributes.GetNamedItem("FramesY").Value);
            }
            if (flipbook.Attributes.GetNamedItem("StartFrame") != null)
            {
                StartFrame = int.Parse(flipbook.Attributes.GetNamedItem("StartFrame").Value);
            }
            if (flipbook.Attributes.GetNamedItem("FrameSequence") != null)
            {
                FrameSequence = flipbook.Attributes.GetNamedItem("FrameSequence").Value;
            }
        }

        int currentFrame = 0;
        //int widthCount = 8;
        //int heightCount = 8;
        int cellHeight = 256;
        int cellWidth = 256;
        Date timeStart = Date.Now;
        bool playing = true;
        public override void Play()
        {
            playing = true;
            timeStart = Date.Now;
        }
        public override void Pause()
        {
            playing = false;
        }
        public override void Stop()
        {
            playing = false;
            currentFrame = 0;
        }

        override public void InitiaizeGeometry()
        {
            int frameCount = frames;
            if (!String.IsNullOrEmpty(frameSequence))
            {
                frameCount = framesList.Count;
            }

            if (playing)
            {
                // todo allow play backwards loop to point.
                int ts = Date.Now - timeStart;
                switch (loopType)
                {
                    case LoopTypes.Loop:
                        currentFrame = (int)((ts / 1000.0 * 24.0) % frameCount) + startFrame;
                        break;
                    case LoopTypes.UpDown:
                        currentFrame = Math.Abs((int)((ts / 1000.0 * 24.0 + frameCount) % (frameCount * 2 - 1)) - (frameCount - 1)) + startFrame;
                        if (currentFrame < 0 || currentFrame > frameCount - 1)
                        {
                            int p = 0;
                        }
                        break;
                    case LoopTypes.Down:
                        currentFrame = Math.Max(0, frameCount - (int)((ts / 1000.0 * 24.0) % frameCount)) + startFrame;
                        break;
                    case LoopTypes.UpDownOnce:
                        int temp = (int)Math.Min(ts / 1000.0 * 24.0, frameCount * 2 + 1) + frameCount;
                        currentFrame = Math.Abs((int)((temp) % (frameCount * 2 - 1)) - (frameCount - 1)) + startFrame;
                        break;
                    case LoopTypes.Once:
                        currentFrame = Math.Min(frameCount - 1, (int)((ts / 1000.0 * 24.0)));
                        break;
                    case LoopTypes.Begin:
                        currentFrame = startFrame;
                        break;
                    case LoopTypes.End:
                        currentFrame = (frameCount - 1) + startFrame;
                        break;
                    default:
                        currentFrame = startFrame;
                        break;
                }

            }
            if (!String.IsNullOrEmpty(frameSequence))
            {
                if (currentFrame < framesList.Count && currentFrame > -1)
                {
                    currentFrame = framesList[currentFrame];
                }
                else
                {
                    currentFrame = 0;
                }
            }

            currentRotation = 0;
        }
    }
}


