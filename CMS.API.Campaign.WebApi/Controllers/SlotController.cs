﻿using CMS.API.Campaign.Application.Services;
using CMS.API.Campaign.Infrastructure.Logging;
using CMS.API.Campaign.Infrastructure.Metric;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CMS.API.Campaign.Application.Models;

namespace CMS.API.Campaign.WebApi.Controllers
{
    /// <summary>
    /// slot api for campaigns
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : Controller
    {
        private readonly IMetricClient _metricClient;
        private readonly ISlotService _slotService;

        /// <summary>
        /// initialize
        /// </summary>
        public SlotController(IMetricClient metricClient, ISlotService slotService)
        {
            _metricClient = metricClient;
            _slotService = slotService;
        }

        //api/Slot?platform=website&location=homePagePromo&language=en-us&country=us
        /// <summary>
        /// Get campaigns for one slot with conditions
        /// </summary>
        /// <param name="platform">Required, Should be one of "website, mobile, app"</param>
        /// <param name="location">Required, Location of Slot, Example: HomepageCategory</param>
        /// <param name="language">Required, Language Code, Example: en-US</param>
        /// <param name="country">Required, Country Code, Example : US</param>
        /// <param name="categoryId">Optional, Category Code, Example : Pets</param>
        /// <param name="promoId">Optional, Promote ID, Example: 070819WD10915</param>
        /// <param name="store">Optional, default value is iHerb</param>
        [HttpGet("/api/Slot")]
        public ActionResult<List<SlotInfo>> GetSlots(string platform, string location, string language, string country,
            string categoryId, string promoId, string store)
        {
            var stop = new Stopwatch();
            stop.Start();
            var methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                using (_metricClient.Timer(methodName))
                {
                    _metricClient.Counter(methodName);
                    if (string.IsNullOrEmpty(store))
                        store = "iHerb";
                    var slots = _slotService.GetSlots(platform, location, country, language, categoryId, promoId,
                        store);
                    stop.Stop();
                    Console.WriteLine($"[SlotController][GetSlots] Return {slots.Count} records, elapsed = {stop.ElapsedMilliseconds}ms, {stop.ElapsedTicks}ts.");
                    return slots;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Error(ex, "[SlotController][GetSlots]");
                return new List<SlotInfo>();
            }
        }

        /// <summary>
        /// Get Preview Slots
        /// </summary>
        [HttpGet("/api/Slot/Preview")]
        public ActionResult<List<SlotInfo>> GetPreviewSlots(string platform, string location, string language, string country,
            string categoryId, string promoId, string store)
        {
            var stop = new Stopwatch();
            stop.Start();
            var methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                using (_metricClient.Timer(methodName))
                {
                    _metricClient.Counter(methodName);
                    if (string.IsNullOrEmpty(store))
                        store = "iHerb";
                    var slots = _slotService.GetSlots(platform, location, country, language, categoryId, promoId,
                        store, true);
                    stop.Stop();
                    Console.WriteLine($"[SlotController][GetPreviewSlots] Return {slots.Count} records, elapsed = {stop.ElapsedMilliseconds}ms.");
                    return slots;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.Error(ex, "[SlotController][GetPreviewSlots]");
                return new List<SlotInfo>();
            }
        }
    }
}