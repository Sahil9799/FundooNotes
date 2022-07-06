﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabasLayer.Note
{
    public class NoteReminderModel
    {
        [Required]
        [RegularExpression("^([0-9]{4})-?(1[0-2]|0[1-9])-?(3[01]|0[1-9]|[12][0-9]) (2[0-3]|[01][0-9]):?([0-5][0-9]):?([0-5][0-9])$")]
        public string Reminder { get; set; }
    }
}