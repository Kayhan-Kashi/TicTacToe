using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Dal.DataModel
{
    public class GameInvitation
    {
        public Guid Id { get; set; }
        public string EmailTo { get; set; }
        public string InvitedBy { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}
