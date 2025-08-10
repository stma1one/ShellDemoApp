using JokesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskBaseApp.Models;
using TaskBaseApp.Services;

namespace TaskBaseApp.ViewModels
{
	/// <summary>
	/// מודל תצוגה עבור עמוד הבדיחה.
	/// </summary>
	public class JokePageViewModel : ViewModelBase
	{
		#region שדות פרטיים
		private readonly JokeServiceApi service; // שירות הבדיחות
		private Joke? joke; // אובייקט הבדיחה הנוכחי
		private string? setup; // חלק ראשון של הבדיחה
		private string? delivery; // חלק שני של הבדיחה
		private bool isDeliveryRevealed = false; // האם התשובה נחשפה
		#endregion

		#region תכונות ציבוריות
		/// <summary>
		/// האם להציג את החלק השני של הבדיחה (רק אם זו בדיחה דו-חלקית).
		/// </summary>
		public bool IsVisible
		{
			get => joke is TwoPartJoke;
		}

		/// <summary>
		/// חלק ראשון של הבדיחה.
		/// </summary>
		public string? SetUp
		{
			get => setup; set
			{
				setup = value; OnPropertyChanged();
			}
		}

		/// <summary>
		/// האם התשובה נחשפה.
		/// </summary>
		public bool IsDeliveryRevealed
		{
			get => isDeliveryRevealed;
			set
			{
				isDeliveryRevealed = value; OnPropertyChanged();
			}
		}

		/// <summary>
		/// חלק שני של הבדיחה.
		/// </summary>
		public string? Delivery
		{
			get => delivery; set
			{
				delivery = value; OnPropertyChanged();
			}
		}
		#endregion

		#region פקודות
		/// <summary>
		/// פקודה לחשיפת התשובה (החלק השני של הבדיחה).
		/// </summary>
		public ICommand RevealDeliveryCommand { get; }

		/// <summary>
		/// פקודה לקבלת בדיחה חדשה.
		/// </summary>
		public ICommand GetJokeCommand { get; private set; }

		#endregion

		#region בנאי
		/// <summary>
		/// בנאי המודל, מקבל שירות בדיחות.
		/// </summary>
		public JokePageViewModel(JokeServiceApi service)
		{
			this.service = service;
			GetJokeCommand = new Command(async () =>
			{
				joke = await service.GetRandomJoke();
				if (joke is OneLiner)
				{
					SetUp = ((OneLiner)joke).Joke;
				}
				if (joke is TwoPartJoke)
				{
					SetUp = ((TwoPartJoke)joke).Setup;
					Delivery = ((TwoPartJoke)joke).Delivery;
				}
				OnPropertyChanged(nameof(IsVisible));
				IsDeliveryRevealed = false; // איפוס חשיפת התשובה כאשר מתקבלת בדיחה חדשה
			});
			RevealDeliveryCommand = new Command(() => IsDeliveryRevealed = !IsDeliveryRevealed);
			
		}
		#endregion

		
	}
}
