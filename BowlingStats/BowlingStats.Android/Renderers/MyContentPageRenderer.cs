using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BowlingStats;
using BowlingStats.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyContentPage), typeof(MyContentPageRenderer))]
namespace BowlingStats.Droid
{
    public class MyContentPageRenderer : VisualElementRenderer<ContentPage>
    {
        public MyContentPageRenderer(Context context) : base(context)
        {
            base.SetBackgroundResource(2131165277);  // numero che rappresenta l'immagine bowling del resource designer.cs
        }
    }

    //public class ButtonRenderer : VisualElementRenderer<Xamarin.Forms.Button>
    //{
    //    public ButtonRenderer(Context context) : base(context)
    //    {

    //    }

    //    public override TextDirection TextDirection { get => 
    //            TextDirection.Rtl;
    //        set => base.TextDirection = value; }
    //}

    //    public class LabelRenderer : VisualElementRenderer<Xamarin.Forms.Label>
    //    {
    //        public LabelRenderer(Context context) : base(context)
    //        {

    //        }

    //        public override TextDirection TextDirection
    //        {
    //            get =>
    //TextDirection.Rtl;
    //            set => base.TextDirection = value;
    //        }

    //        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
    //        {
    //            base.OnElementChanged(e);

    //            var a = 1;
    //        }
    //    }
    //public class MyDatePickerRenderer : DatePickerRenderer
    //{
    //    public MyDatePickerRenderer(Context context) : base(context)
    //    {
    //    }

    //    protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
    //    {
    //        base.SetBackgroundColor(Android.Graphics.Color.Red);
    //        base.BackgroundTintMode = PorterDuff.Mode.Overlay;
    //        return base.CreateDatePickerDialog(year, month, day);
    //    }

    //    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
    //    {
    //        base.OnElementChanged(e);
    //        //base.Visibility = ViewStates.Invisible;
    //        //base.TextDirection = TextDirection.Rtl;
    //    }

    //    protected override void OnDraw(Canvas canvas)
    //    {
    //        base.OnDraw(canvas);

    //        //base.SetBackgroundColor(Android.Graphics.Color.Red);
    //    }

    //    public override bool Selected
    //    {
    //        get => base.Selected;
    //        set => base.Selected = value;
    //    }


    
}