﻿using System.Threading.Tasks;
using System.Collections.Generic;

namespace TextMood
{
	abstract class TextResultsService : BaseHttpClientService
    {
		public static Task<List<TextMoodModel>> GetTextModels() =>
			GetDataObjectFromAPI<List<TextMoodModel>>(BackendConstants.GetEmotionResultsAPIUrl);
    }
}
