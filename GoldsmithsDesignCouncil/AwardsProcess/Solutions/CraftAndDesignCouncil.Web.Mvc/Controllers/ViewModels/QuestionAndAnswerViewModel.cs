﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels
{
    public class QuestionAndAnswerViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
    }
}