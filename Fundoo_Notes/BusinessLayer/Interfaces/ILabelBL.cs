using DatabasLayer.Label;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {
        Task AddLabel(int userid, int noteid, string labelName);
        Task DeleteLabel(int userid, int noteid);
        Task UpdateLabel(int userid, int noteid, string labelName);
        Task<Label> GetLabel(int userid, int noteid);

        Task<List<Label>> GetAllLabel(int userid);
        Task<List<LabelResponseModel>> GetLabel_Join(int userid);
    }
}