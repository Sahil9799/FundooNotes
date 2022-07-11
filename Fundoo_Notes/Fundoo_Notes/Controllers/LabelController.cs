using BusinessLayer.Interfaces;
using DatabasLayer.Label;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundoo_Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        ILabelBL labelBL;
        FundooContext fundooContext;
        public LabelController(ILabelBL labelBL, FundooContext fundooContext)
        {
            this.labelBL = labelBL;
            this.fundooContext = fundooContext;
        }

        [Authorize]
        [HttpPost("addLabel/{noteId}/{labelName}")]
        public async Task<ActionResult> addLabel(int noteId, string labelName)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);

                if (label != null)
                {
                    return this.BadRequest(new { status = 301, success = false, Message = "Enter different Noteid" });
                }
                await this.labelBL.AddLabel(UserId, noteId, labelName);
                return this.Ok(new { success = true, Message = "Label Created Successfully!" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        //[Authorize]
        //[HttpPost("addLabel/{noteId}/{labelName}")]
        //public async Task<ActionResult> addLabel(int noteId, string labelName)
        //{
        //    try
        //    {
        //        var currentUser = HttpContext.User;
        //        int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

        //        var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.noteId == noteId);

        //        if (note == null)
        //        {
        //            return this.BadRequest(new { success = false, Message = "Note doesn't Exists" });
        //        }
        //        await this.labelBL.AddLabel(UserId, noteId, labelName);
        //        return this.Ok(new { success = true, Message = "Label is added" });
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [Authorize]
        [HttpDelete("DeleteLabel/{noteId}")]

        public async Task<ActionResult> DeleteLabel(int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                //var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.noteId == noteId);
                //if (note == null)
                //{
                //    return this.BadRequest(new { success = false, Message = "Note Doesn't Exits" });
                //}

                if (label == null)
                {
                    return this.BadRequest(new { success = false, Message = "Label Doesn't Exists" });
                }
                await this.labelBL.DeleteLabel(UserId, noteId);
                return this.Ok(new { success = true, Message = "Label Deleted Successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("UpdateLabel/{noteId}")]

        public async Task<ActionResult> UpdateLabel(int noteId, string LabelName)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);


                var label = fundooContext.Labels.Where(u => u.UserId == UserId && u.NoteId == noteId).FirstOrDefault();
                if (label == null)
                {
                    return this.BadRequest(new { success = true, Message = "Label Doesn't Exists" });
                }
                await this.labelBL.UpdateLabel(UserId, noteId, LabelName);
                return this.Ok(new { success = true, Message = "Label Updated successfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("GetLabel/{noteId}")]
        public async Task<ActionResult> GetLabel(int noteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var label = fundooContext.Labels.FirstOrDefault(x => x.UserId == UserId && x.NoteId == noteId);
                if (label == null)
                {
                    return this.BadRequest(new { success = false, Message = "Label Doesn't Exists" });
                }

                var label1 = await this.labelBL.GetLabel(UserId, noteId);
                return this.Ok(new { success = true, Message = $"Label Obtained Successfully for {label.LabelName}", data = label1 });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("GetAllLabel")]

        public async Task<ActionResult> GetAllLabel()
        {
            try
            {
                var currentUser = HttpContext.User;
                var UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == UserId);
                if (label == null)
                {
                    this.BadRequest(new { success = false, Message = "Label doesn't exist" });
                }
                List<Label> labelList = new List<Label>();
                labelList = await this.labelBL.GetAllLabel(UserId);
                return Ok(new { success = true, Message = $"Note Obtained successfully ", data = labelList });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    [Authorize]
    [HttpGet("GetLabel_Join")]

    public async Task<ActionResult> GetLabel_Join()
    {
        try
        {
            var currentUser = HttpContext.User;
            var UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

            var label = fundooContext.Labels.FirstOrDefault(u => u.UserId == UserId);
            if (label == null)
            {
                this.BadRequest(new { success = false, Message = "Label doesn't exist" });
            }
            List<LabelResponseModel> labelList = new List<LabelResponseModel>();
            labelList = await this.labelBL.GetLabel_Join(UserId);
            return Ok(new { success = true, Message = $"Note Obtained successfully ", data = labelList });
        }
        catch (Exception e)
        {
            throw e;
        }
    }

}

