using CMS.API.Campaign.Application.Models;
using CMS.API.Campaign.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using CMS.API.Campaign.Domain.Entities;

namespace CMS.API.Campaign.Application.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly SlotImageConfig _imageConfig;
        private readonly ICacheService _cacheService;

        public SlotService(ISlotRepository slotRepository, IOptions<SlotImageConfig> config, ICacheService cacheService)
        {
            _slotRepository = slotRepository;
            _imageConfig = config.Value;
            _cacheService = cacheService;
        }

        public List<SlotInfo> GetSlots(string platform, string location, string country, string language,
            string categoryId, string promoId, string store, bool preview = false)
        {
            if (string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(language) ||
                string.IsNullOrEmpty(store) || string.IsNullOrEmpty(country))
                return new List<SlotInfo>();

            var key = $"{store.ToLower()}-{platform.ToLower()}-{location.ToLower()}-{language.ToLower()}";
            var memoryKey = $"{key}-{country}-{categoryId}-{promoId}";

            var result = _cacheService.Get(memoryKey);
            if (result?.Count > 0)
                return result;

            var stop = new Stopwatch();
            stop.Start();
            var slots = _slotRepository.GetSlots(key, preview);
            stop.Stop();
            Console.WriteLine($"[SlotService][GetSlots] Read redis, elapsed = {stop.ElapsedMilliseconds}ms.");

            if (slots.Count == 0)
                return new List<SlotInfo>();

            result = GetSlotInfoByCondition(slots, country, categoryId, promoId);

            _cacheService.Set(memoryKey, result);

            return result;
        }

        private List<SlotInfo> GetSlotInfoByCondition(IEnumerable<SlotEntity> slots, string country, string categoryId,
            string promoId)
        {
            var result = new List<SlotInfo>();

            foreach (var slot in slots)
            {
                if (IsCountryInExclude(slot.CountryExclude, country))
                    continue;

                if (!IsCountryInSpecific(slot.CountrySpecific, country))
                    continue;

                if (!string.IsNullOrEmpty(categoryId) &&
                    !slot.CategoryId.Equals(categoryId, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (!string.IsNullOrEmpty(promoId) &&
                    !slot.PromoCode.Equals(promoId, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (country.Equals("CN", StringComparison.CurrentCultureIgnoreCase))
                {
                    slot.HtmlLayout = slot.HtmlLayout?.Replace(_imageConfig.DefaultHeader, _imageConfig.ChinaHeader);
                }

                result.Add(new SlotInfo()
                {
                    Id = slot.Id,
                    Title = slot.Title,
                    HtmlLayout = slot.HtmlLayout,
                    Url = slot.LandingUrl,
                    Subtitle = slot.Subtitle,
                    AltText = slot.AltText,
                    EndDate = UtcTime2PstTime(slot.EndDate).ToString("yyyy-MM-dd HH:mm:ss")
                });
            }

            return result;
        }

        private static bool IsCountryInExclude(string countryExclude, string country)
        {
            if (string.IsNullOrEmpty(countryExclude))
                return false;

            if (countryExclude.Contains(','))
            {
                return countryExclude.Replace(" ", "").Split(',')
                    .Any(ex => ex.Equals(country, StringComparison.CurrentCultureIgnoreCase));
            }

            return countryExclude.Equals(country, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool IsCountryInSpecific(string countrySpecific, string country)
        {
            if (string.IsNullOrEmpty(countrySpecific) || countrySpecific == "Global")
                return true;

            if (countrySpecific.Contains(','))
            {
                return countrySpecific.Replace(" ", "").Split(',').Where(spe => !string.IsNullOrEmpty(spe)).Any(spe =>
                    spe.Equals(country, StringComparison.CurrentCultureIgnoreCase));
            }

            return countrySpecific.Equals(country, StringComparison.CurrentCultureIgnoreCase);
        }

        private static DateTime UtcTime2PstTime(DateTime utcTime)
        {
            var tmpExact = DateTime.ParseExact(utcTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var utc = TimeZoneInfo.ConvertTimeToUtc(tmpExact, TimeZoneInfo.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, GetPacificStandardTime());
        }

        private static TimeZoneInfo GetPacificStandardTime()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
            }
        }
    }
}
