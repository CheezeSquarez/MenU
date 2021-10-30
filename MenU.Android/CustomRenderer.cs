﻿using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MenU.Refreneces.Controls.CustomEntry), typeof(MenU.Droid.NoUnderlineEntry))]
namespace MenU.Droid
{
    public class NoUnderlineEntry : EntryRenderer
    {
        public NoUnderlineEntry(Context c) : base(c) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                this.Control.SetBackground(null);
                this.Control.SetPadding(35, 0, 0, 0);
                this.Control.TextSize = 18;
            }
        }
    }
}