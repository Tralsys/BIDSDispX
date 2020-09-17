﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace TR.BIDSDispX.SampleView
{
	public class ScaleLabelView : Grid, IAngleAndValMinMax
	{
		#region Properties
		private double __MaxValAngle = 210;
		/// <summary>最大値をとるときの目盛の角度[deg]  ←:0, ↑:90, →:180</summary>
		public double MaxValAngle
		{
			get => __MaxValAngle;
			set
			{
				if (__MaxValAngle == value)
					return;

				__MaxValAngle = value;
				PropUpdated();
			}
		}

		private double __MinValAngle = -30;
		/// <summary>最小値をとるときの目盛の角度[deg]  ←:0, ↑:90, →:180</summary>
		public double MinValAngle
		{
			get => __MinValAngle;
			set
			{
				if (__MinValAngle == value)
					return;

				__MinValAngle = value;
				PropUpdated();
			}
		}

		private double __MinValue = 0;
		/// <summary>表示の最小値[km/h]</summary>
		public double MinValue
		{
			get => __MinValue;
			set
			{
				if (__MinValue == value)
					return;

				__MinValue = value;
				PropUpdated();
			}
		}

		private double __MaxValue = 160;
		/// <summary>表示の最大値[km/h]</summary>
		public double MaxValue
		{
			get => __MaxValue;
			set
			{
				if (__MaxValue == value)
					return;

				__MaxValue = value;
				PropUpdated();
			}
		}

		private int __LabelStep = 10;
		/// <summary>文字を配置する間隔[km/h]</summary>
		public int LabelStep
		{
			get => __LabelStep;
			set
			{
				if (__LabelStep == value)
					return;

				__LabelStep = value;
				PropUpdated();
			}
		}

		private Color __TextColor = Color.Black;
		/// <summary>目盛の色</summary>
		public Color TextColor
		{
			get => __TextColor;
			set
			{
				if (__TextColor == value)
					return;

				__TextColor = value;
				PropUpdated();
			}
		}

		private double __Radius = 80;
		public double Radius
		{
			get => __Radius;
			set
			{
				if (__Radius == value)
					return;

				__Radius = value;
				HeightRequest = WidthRequest = __Radius * 2;

				PropUpdated();
			}
		}

		private double __FontSize = 16;
		public double FontSize
		{
			get => __FontSize;
			set
			{
				if (__FontSize == value)
					return;

				__FontSize = value;
				PropUpdated();
			}
		}

		private string __FontFamily = string.Empty;
		public string FontFamily
		{
			get => __FontFamily;
			set
			{
				if (__FontFamily == value)
					return;

				__FontFamily = value;
				PropUpdated();
			}
		}

		private FontAttributes __FontAttributes = FontAttributes.None;
		public FontAttributes FontAttributes
		{
			get => __FontAttributes;
			set
			{
				if (__FontAttributes == value)
					return;

				__FontAttributes = value;
				PropUpdated();
			}
		}

		#endregion

		public ScaleLabelView()
		{
			PropUpdated();
		}

		private bool PropUpdateAlreadyRequested = false;
		public void PropUpdated()
		{
			if (PropUpdateAlreadyRequested)
				return;//二重で処理する必要はない

			PropUpdateAlreadyRequested = true;

			MainThread.BeginInvokeOnMainThread(() =>
			{
				Children?.Clear();//既存の表示を削除

				for (int i = (int)MinValue; i <= MaxValue; i += LabelStep)
				{
					double angle = ((double)i).GetAngle(this);
					ContentView v = new ContentView
					{
						Content = new Label
						{
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Center,
							AnchorX = 0.5,
							AnchorY = 0.5,
							TextColor = this.TextColor,
							FontSize = this.FontSize,
							FontFamily = this.FontFamily,
							FontAttributes = this.FontAttributes,
							Rotation = -angle,
							Text = i.ToString()
						},

						AnchorY = 0.5,
						BackgroundColor = Color.Transparent,
						HorizontalOptions = LayoutOptions.Start,
						VerticalOptions = LayoutOptions.Start,
						Rotation = angle
					};
					v.SizeChanged += (s, e) =>
					{
						ContentView cv = s as ContentView;

						cv.Margin = new Thickness(0, Radius - (cv.Height / 2), 0, 0);
						cv.AnchorX = Radius / cv.Width;
					};

					this.Children.Add(v);
				}

				PropUpdateAlreadyRequested = false;//処理完了済を記録
			});

		}

	}
}
